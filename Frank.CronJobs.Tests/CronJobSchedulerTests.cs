using Frank.CronJobs.Cron;
using Frank.Testing.TestBases;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Frank.CronJobs.Tests;

public class CronJobSchedulerTests(ITestOutputHelper outputHelper) : HostApplicationTestBase(outputHelper)
{
    /// <inheritdoc />
    protected override Task SetupAsync(HostApplicationBuilder builder)
    {
        builder.Services.AddCronJob<MyService>(PredefinedCronExpressions.EverySecond);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Test()
    {
        await Task.Delay(5000);
    }
    
    private class MyService(ILogger<MyService> logger) : ICronJob
    {
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Running");
            await Task.Delay(100, cancellationToken);
        }
    }
}