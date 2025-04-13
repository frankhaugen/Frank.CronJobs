using FluentAssertions;
using Frank.CronJobs.Cron;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frank.CronJobs.Tests.ScheduleMaintainer;

public class ScheduleMaintainerChangeScheduleTests
{
    private readonly List<DateTime> _runTimes = new();
    private IHost _host = null!;

    [Before(HookType.Test)]
    public void SetupHost()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Services.AddCronJob<MyService>(PredefinedCronExpressions.EverySecond);
        builder.Services.AddSingleton(_runTimes);
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
    public async Task Test()
    {
        await Task.Delay(1500);
        _runTimes.Should().HaveCount(1);
        await Task.Delay(6000);
        _runTimes.Should().HaveCountGreaterThan(1);
    }
    
    private class MyService(ILogger<MyService> logger, List<DateTime> runTimes, IScheduleMaintainer maintainer) : ICronJob
    {
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Running");
            runTimes.Add(DateTime.UtcNow);
            await Task.Delay(250, cancellationToken);
            maintainer.SetSchedule<MyService>(PredefinedCronExpressions.EveryFiveSeconds);
        }
    }
}
