using Frank.CronJobs.Cron;
using Frank.CronJobs.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Frank.CronJobs;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a cron job to the service collection.
    /// </summary>
    /// <typeparam name="TCronJob">The type of the cron job.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="cronExpression">The cron expression.</param>
    /// <param name="timeZoneIanaName">The IANA name of the time zone.</param>
    /// <param name="running">Whether the cron job should be running or not. Default is true.</param>
    public static void AddCronJob<TCronJob>(this IServiceCollection services, string cronExpression, string timeZoneIanaName, bool running = true) where TCronJob : class, ICronJob
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneIanaName);
        AddCronJob<TCronJob>(services, cronExpression, running, timeZone);
    }

    /// <summary>
    /// Adds a cron job to the service collection.
    /// </summary>
    /// <typeparam name="TCronJob">The type of the cron job.</typeparam>
    /// <param name="services">The service collection to add the cron job to.</param>
    /// <param name="cronExpression">The cron expression that defines the schedule of the job.</param>
    /// <param name="running">A flag indicating whether the cron job should initially be running. The default value is true.</param>
    /// <param name="timeZone">The time zone for the cron job's schedule. The default value is null, which represents the UTC time zone.</param>
    /// <exception cref="InvalidCronExpressionException">Thrown when the provided cron expression is invalid.</exception>
    public static void AddCronJob<TCronJob>(this IServiceCollection services, string cronExpression, bool running = true, TimeZoneInfo? timeZone = null) where TCronJob : class, ICronJob
    {
        timeZone ??= TimeZoneInfo.Utc;

        if (!CronHelper.IsValid(cronExpression))
        {
            throw new InvalidCronExpressionException(cronExpression);
        }
        
        var descriptor = new CronJobDescriptor(typeof(TCronJob), cronExpression, running, timeZone);

        // Add the descriptor for this cron job
        services.AddSingleton<ICronJobDescriptor>(descriptor);

        // Register the cron job service
        services.AddKeyedSingleton<ICronJob, TCronJob>(descriptor.Name);
        
        // Register the cron job scheduler if not already present as a hosted service
        if (!services.Any(d => d.ServiceType == typeof(IHostedService) && d.ImplementationType == typeof(CronJobScheduler)))
            services.AddHostedService<CronJobScheduler>();
        
        // Register the schedule maintainer if not already present
        if (services.All(d => d.ServiceType != typeof(IScheduleMaintainer)))
            services.AddSingleton<IScheduleMaintainer, ScheduleMaintainer>();
    }
}