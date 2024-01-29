using Frank.CronJobs.Cron;

namespace Frank.CronJobs;

/// <summary>
/// Represents a builder for configuring cron jobs.
/// </summary>
public interface ICronJobsBuilder
{
    /// <summary>
    /// Adds a cron job of type <typeparamref name="T"/> to the cron jobs builder. The cron job will be configured using the given action.
    /// </summary>
    /// <typeparam name="T">The type of the cron job.</typeparam>
    /// <param name="configure">An action to configure the cron job options.</param>
    /// <returns>The cron jobs builder instance.</returns>
    ICronJobsBuilder AddCronJob<T>(Action<CronJobOptions> configure) where T : class, ICronJob;

    /// <summary>
    /// Adds a cron job of type <typeparamref name="T"/> to the cron jobs builder with the specified options.
    /// </summary>
    /// <typeparam name="T">The type of the cron job to add.</typeparam>
    /// <param name="options">The options for the cron job.</param>
    /// <returns>The updated instance of the cron jobs builder.</returns>
    ICronJobsBuilder AddCronJob<T>(CronJobOptions options) where T : class, ICronJob;

    /// <summary>
    /// Adds a cron job of type T to the cron jobs builder with the specified cron expression.
    /// </summary>
    /// <typeparam name="T">The type of the cron job to add. Must implement the ICronJob interface.</typeparam>
    /// <param name="cron">The cron expression to use for scheduling the cron job.</param>
    /// <returns>An instance of ICronJobsBuilder for method chaining.</returns>
    ICronJobsBuilder AddCronJob<T>(string cron) where T : class, ICronJob;

    /// <summary>
    /// Adds a cron job to the cron jobs builder.
    /// </summary>
    /// <typeparam name="T">The type of cron job.</typeparam>
    /// <param name="cronExpression">The cron expression to schedule the job.</param>
    /// <returns>The same cron jobs builder instance.</returns>
    ICronJobsBuilder AddCronJob<T>(CronExpression cronExpression) where T : class, ICronJob;
}