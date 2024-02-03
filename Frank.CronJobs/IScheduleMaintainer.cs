namespace Frank.CronJobs;

/// <summary>
/// Provides methods to maintain and manage cron job schedules.
/// </summary>
public interface IScheduleMaintainer
{
    /// <summary>
    /// Sets the cron expression for a specified cron job.
    /// </summary>
    /// <typeparam name="T">The type of the cron job.</typeparam>
    /// <param name="cronExpression">The cron expression to set. Must be a valid cron expression.</param>
    /// <exception cref="InvalidCronExpressionException">
    /// Thrown when the provided cron expression is not valid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no cron job with the specified name is found.
    /// </exception>
    void SetSchedule<T>(string cronExpression) where T : ICronJob;

    /// <summary>
    /// Sets the time zone for a specified cron job.
    /// </summary>
    /// <typeparam name="T">The type of the cron job.</typeparam>
    /// <param name="timeZoneInfo">The <see cref="TimeZoneInfo"/> representing
    /// the time zone to set.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no cron job with the specified name is found.
    /// </exception>
    void SetTimeZone<T>(TimeZoneInfo timeZoneInfo) where T : ICronJob;

    /// <summary>
    /// Stops a specified cron job.
    /// </summary>
    /// <typeparam name="T">The type of the cron job to stop.</typeparam>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no cron job with the specified name is found.
    /// </exception>
    void Stop<T>() where T : ICronJob;

    /// <summary>
    /// Starts a specified cron job.
    /// </summary>
    /// <typeparam name="T">The type of the cron job to start.</typeparam>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no cron job with the specified name is found.
    /// </exception>
    void Start<T>() where T : ICronJob;
}