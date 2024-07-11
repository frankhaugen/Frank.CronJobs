namespace Frank.CronJobs.Internals;

/// <inheritdoc />
internal class CronJobDescriptor(Type type, string cronExpression, bool running, TimeZoneInfo timeZoneInfo) : ICronJobDescriptor
{
    /// <inheritdoc />
    public string Name { get; } = type.GetFullDisplayName();

    /// <inheritdoc />
    public string Schedule { get; set; } = cronExpression;

    /// <inheritdoc/>
    public bool Running { get; set; } = running;

    /// <inheritdoc/>
    public TimeZoneInfo TimeZoneInfo { get; set; } = timeZoneInfo;
}
