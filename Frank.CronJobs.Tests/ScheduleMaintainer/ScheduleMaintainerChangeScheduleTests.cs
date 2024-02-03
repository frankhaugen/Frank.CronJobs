using FluentAssertions;
using Frank.CronJobs.Cron;
using Frank.Testing.TestBases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Frank.CronJobs.Tests.ScheduleMaintainer;

public class ScheduleMaintainerChangeScheduleTests(ITestOutputHelper outputHelper) : HostApplicationTestBase(outputHelper)
{
    private readonly List<DateTime> _runTimes = new();

    /// <inheritdoc />
    protected override Task SetupAsync(HostApplicationBuilder builder)
    {
        builder.Services.AddCronJob<MyService>(PredefinedCronExpressions.EverySecond);
        builder.Services.AddSingleton(_runTimes);
        return Task.CompletedTask;
    }

    [Fact]
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