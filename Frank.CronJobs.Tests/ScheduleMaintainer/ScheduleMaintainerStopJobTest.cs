using FluentAssertions;
using Frank.CronJobs.Cron;
using Frank.Testing.TestBases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Frank.CronJobs.Tests.ScheduleMaintainer;

public class ScheduleMaintainerStopJobTest(ITestOutputHelper outputHelper) : HostApplicationTestBase(outputHelper)
{
    private readonly List<Guid> _runIds = new();

    /// <inheritdoc />
    protected override Task SetupAsync(HostApplicationBuilder builder)
    {
        builder.Services.AddCronJob<MyStoppingService>(PredefinedCronExpressions.EverySecond);
        builder.Services.AddSingleton(_runIds);
        return Task.CompletedTask;
    }

    [Fact]
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