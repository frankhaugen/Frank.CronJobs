using Frank.CronJobs.Cron;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frank.CronJobs.Tests;

public class CronJobSchedulerTests
{
    private IHost _host = null!;

    [Before(HookType.Test)]
    public void SetupHost()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Services.AddCronJob<MyService>(PredefinedCronExpressions.EverySecond);
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
