using Frank.CronJobs.Cron;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frank.CronJobs.Internals;

internal  class CronJobScheduler(IServiceScopeFactory serviceScopeFactory, ILogger<CronJobScheduler> logger, IEnumerable<ICronJobDescriptor> cronJobDescriptors, IScheduleMaintainer scheduleMaintainer) : IHostedService
{
    private readonly List<JobInterval> _jobIntervals = new();
    private readonly ScheduleMaintainer _scheduleMaintainer = scheduleMaintainer as ScheduleMaintainer ?? throw new InvalidOperationException("ScheduleMaintainer is not of type ScheduleMaintainer");

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting cron job scheduler...");
        
        logger.LogInformation("Found {Count} cron job descriptors", cronJobDescriptors.Count());

        foreach (var descriptor in cronJobDescriptors.Where(descriptor => descriptor.Running))
        {
            if (!CronHelper.IsValid(descriptor.Schedule))
            {
                logger.LogWarning("Invalid cron expression for {DescriptorName}", descriptor.Name);
                continue;
            }
            
            logger.LogDebug("Starting cron job {DescriptorName}", descriptor.Name);
            
            var jobInterval = new JobInterval(descriptor, () => ExecuteJobAsync(descriptor, cancellationToken), cancellationToken);
            _jobIntervals.Add(jobInterval);
            jobInterval.Run();
            
            _scheduleMaintainer.ScheduleChanged = changedDescriptor =>
            {
                if (changedDescriptor.Name != descriptor.Name)
                    return;
                if (!CronHelper.IsValid(changedDescriptor.Schedule))
                {
                    logger.LogWarning("Invalid cron expression for {DescriptorName}", changedDescriptor.Name);
                    return;
                }
                logger.LogInformation("Restarting cron job {DescriptorName}", changedDescriptor.Name);
                jobInterval.Refresh(changedDescriptor);
            };
        }
        
        await Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping cron job scheduler...");
        foreach (var jobInterval in _jobIntervals)
        {
            jobInterval.Dispose();
        }
        _jobIntervals.Clear();

        return Task.CompletedTask;
    }

    private async Task ExecuteJobAsync(ICronJobDescriptor descriptor, CancellationToken cancellationToken)
    {
        if (!descriptor.Running || !CronHelper.IsValid(descriptor.Schedule) || cancellationToken.IsCancellationRequested)
        {
            return;
        }
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        using var loggingScope = logger.BeginScope("CronJob {JobTypeName} with schedule {Schedule} in timezone {TimeZone}", descriptor.Name, descriptor.Schedule, descriptor.TimeZoneInfo.Id);
        
        var jobInstance = scope.ServiceProvider.GetRequiredKeyedService<ICronJob>(descriptor.Name);

        try
        {
            logger.LogDebug("Executing job {JobTypeName}", descriptor.Name);
            await jobInstance.RunAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error executing job {JobTypeName}", descriptor.Name);
        }
    }
}
