using FluentAssertions;
using Frank.CronJobs.Cron;
using Frank.Testing.TestBases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Frank.CronJobs.Tests.ScheduleMaintainer;

public class ScheduleMaintainerRestartJobTest(ITestOutputHelper outputHelper) : HostApplicationTestBase(outputHelper)
{
    private readonly List<Version> _runVersions = new();

    /// <inheritdoc />
    protected override Task SetupAsync(HostApplicationBuilder builder)
    {
        builder.Services.AddCronJob<MyRestartingService>(PredefinedCronExpressions.EverySecond);
        builder.Services.AddSingleton(_runVersions);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Test3()
    {
        await Task.Delay(1500);
        _runVersions.Should().HaveCount(1);
        await Task.Delay(6000);
        _runVersions.Should().HaveCount(1);
        var maintainer = Services.GetRequiredService<IScheduleMaintainer>();
        maintainer.Start<MyRestartingService>();
        await Task.Delay(1000);
        _runVersions.Should().HaveCountGreaterOrEqualTo(2);
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