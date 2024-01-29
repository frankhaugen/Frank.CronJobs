using Frank.Testing.Logging;
using Frank.Testing.TestBases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Frank.CronJobs.Tests.DependencyInjection;

public class ServiceCollectionExtensionsTests(ITestOutputHelper outputHelper) : HostApplicationTestBase(outputHelper)
{
    /// <inheritdoc />
    protected override Task SetupAsync(HostApplicationBuilder builder)
    {
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            {"CronJobRunnerOptions:Running", "true"},
        }!);
        builder.Services.AddCronJobs(builder.Configuration, cronJobBuilder =>
        {
            cronJobBuilder.AddCronJob<MyService>(options =>
            {
                options.Cron = "* * * * * *";
                options.Running = true;
            });
            cronJobBuilder.AddCronJob<MyOtherService>(options =>
            {
                options.Cron = "* * * * * *";
                options.Running = true;
            });
        });
        return Task.CompletedTask;
    }

    [Fact]
    public async Task AddCronJob_WithCronExpression_ShouldRunAsync()
    {
        await Task.Delay(5000);
    }
    
    private class MyService : ICronJob
    {
        private readonly ILogger<MyService> _logger;

        public MyService(ILogger<MyService> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Running");
            await Task.Delay(100);
        }
    }
    
    private class MyOtherService : ICronJob
    {
        private readonly ILogger<MyOtherService> _logger;

        public MyOtherService(ILogger<MyOtherService> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Running other");
            await Task.Delay(100);
        }
    }
}