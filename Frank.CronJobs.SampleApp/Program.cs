// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Frank.CronJobs;
using Frank.CronJobs.Cron;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");

var builder = new HostBuilder();
builder.ConfigureLogging(logging =>
{
    logging.AddDebug().AddConsole();
    // logging.AddJsonConsole(options =>
    // {
    //     options.JsonWriterOptions = new JsonWriterOptions
    //     {
    //         Indented = true,
    //     };
    //     options.IncludeScopes = true;
    //     options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fffffff";
    //     options.UseUtcTimestamp = true;
    // });
});
builder.ConfigureServices((context, services) =>
{
    services.AddCronJob<MyService>(PredefinedCronExpressions.EverySecond);
});

await builder.RunConsoleAsync();

public class MyService(ILogger<MyService> logger) : ICronJob
{
    /// <inheritdoc />
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Running");
        await Task.Delay(100, cancellationToken);
    }
}