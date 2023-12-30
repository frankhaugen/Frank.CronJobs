using Frank.CronJobs.DependencyInjection;
using Frank.CronJobs.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Frank.CronJobs.Tests.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    private readonly ITestOutputHelper _outputHelper;
    public ServiceCollectionExtensionsTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task AddCronJob_WithCronExpression_ShouldRunAsync()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"CronJobRunnerOptions:Running", "true"},
                });
            })
            .ConfigureServices((context, services) =>
            {
                services.AddLogging(builder => builder.AddXunit(_outputHelper));
                services.AddCronJobs(context.Configuration, cronJobBuilder =>
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
            })
            .Build();

        // Act
        await host.RunAsync(cancellationTokenSource.Token);

        // Assert
        _outputHelper.WriteLine("Finished");
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