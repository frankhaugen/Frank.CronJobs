using Frank.CronJobs.Cron;
using Frank.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Frank.CronJobs.Internals;

internal class ScheduleMaintainer(IServiceProvider serviceProvider) : IScheduleMaintainer
{
    /// <inheritdoc />
    public void SetSchedule<T>(string cronExpression) where T : ICronJob
    {
        if (!CronHelper.IsValid(cronExpression))
            throw new InvalidCronExpressionException(cronExpression);
        var descriptors = serviceProvider.GetServices<ICronJobDescriptor>();
        var descriptor = descriptors.FirstOrDefault(d => d.Name == typeof(T).GetFullDisplayName());
        if (descriptor is null)
            throw new InvalidOperationException($"No cron job with name {typeof(T).GetFullDisplayName()} found");
        descriptor.Schedule = cronExpression;
        
        ScheduleChanged?.Invoke(descriptor);
    }

    /// <inheritdoc />
    public void SetTimeZone<T>(TimeZoneInfo timeZoneInfo) where T : ICronJob
    {
        var descriptors = serviceProvider.GetServices<ICronJobDescriptor>();
        var descriptor = descriptors.FirstOrDefault(d => d.Name == typeof(T).GetFullDisplayName());
        if (descriptor is null)
            throw new InvalidOperationException($"No cron job with name {typeof(T).GetFullDisplayName()} found");
        descriptor.TimeZoneInfo = timeZoneInfo;
        
        ScheduleChanged?.Invoke(descriptor);
    }

    /// <inheritdoc />
    public void Stop<T>() where T : ICronJob
    {
        var descriptors = serviceProvider.GetServices<ICronJobDescriptor>();
        var descriptor = descriptors.FirstOrDefault(d => d.Name == typeof(T).GetFullDisplayName());
        if (descriptor is null)
            throw new InvalidOperationException($"No cron job with name {typeof(T).GetFullDisplayName()} found");
        descriptor.Running = false;
        
        ScheduleChanged?.Invoke(descriptor);
    }

    /// <inheritdoc />
    public void Start<T>() where T : ICronJob
    {
        var descriptors = serviceProvider.GetServices<ICronJobDescriptor>();
        var descriptor = descriptors.FirstOrDefault(d => d.Name == typeof(T).GetFullDisplayName());
        if (descriptor is null)
            throw new InvalidOperationException($"No cron job with name {typeof(T).GetFullDisplayName()} found");
        descriptor.Running = true;
        
        ScheduleChanged?.Invoke(descriptor);
    }
    
    public Action<ICronJobDescriptor>? ScheduleChanged;
}