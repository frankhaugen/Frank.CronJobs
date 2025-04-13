using FluentAssertions;
using Frank.CronJobs.Cron;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frank.CronJobs.Tests.ScheduleMaintainer;

public class ScheduleMaintainerStopJobTest
{
    private readonly List<Guid> _runIds = new();
    private IHost _host = null!;

    [Before(HookType.Test)]
    public void SetupHost()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Services.AddCronJob<MyStoppingService>(PredefinedCronExpressions.EverySecond);
        builder.Services.AddSingleton(_runIds);
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

    [Test]
    public async Task Test2()
    {
        await Task.Delay(1500);
        _runIds.Should().HaveCount(1);
        await Task.Delay(6000);
        _runIds.Should().HaveCount(1);
    }
    
    private class MyStoppingService(ILogger<MyStoppingService> logger, List<Guid> runIds, IScheduleMaintainer maintainer) : ICronJob
    {
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Running Stop service");
            runIds.Add(Guid.NewGuid());
            await Task.Delay(250, cancellationToken);
            maintainer.Stop<MyStoppingService>();
        }
    }
}
