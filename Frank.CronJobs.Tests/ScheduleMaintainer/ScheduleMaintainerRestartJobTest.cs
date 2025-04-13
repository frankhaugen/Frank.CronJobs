using FluentAssertions;
using Frank.CronJobs.Cron;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frank.CronJobs.Tests.ScheduleMaintainer;

public class ScheduleMaintainerRestartJobTest
{
    private readonly List<Version> _runVersions = new();
    
    private IHost _host = null!;

    [Before(HookType.Test)]
    public void SetupHost()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Services.AddCronJob<MyRestartingService>(PredefinedCronExpressions.EverySecond);
        builder.Services.AddSingleton(_runVersions);
        builder.Logging.AddDebug();
        _host = builder.Build();
        _host.StartAsync().GetAwaiter().GetResult();
    }
    
    [After(HookType.Test)]
    public void DisposeHost()
    {
        _host.StopAsync().GetAwaiter().GetResult();
        _host.Dispose();
    }
    
    private IServiceProvider Services => _host.Services;

    [Test]
    public async Task Test3()
    {
        await Task.Delay(1500);
        _runVersions.Should().HaveCount(1);
        await Task.Delay(6000);
        _runVersions.Should().HaveCount(1);
        var maintainer = Services.GetRequiredService<IScheduleMaintainer>();
        maintainer.Start<MyRestartingService>();
        await Task.Delay(1000);
        _runVersions.Should().HaveCountGreaterThanOrEqualTo(2);
    }
    
    private class MyRestartingService(ILogger<MyRestartingService> logger, List<Version> runVersions, IScheduleMaintainer maintainer) : ICronJob
    {
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Running restart service");
            runVersions.Add(new Version(1, 0, 0, 0));
            await Task.Delay(250, cancellationToken);
            maintainer.Stop<MyRestartingService>();
        }
    }
}
