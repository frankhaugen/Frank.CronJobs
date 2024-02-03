using Frank.Reflection;

namespace Frank.CronJobs.Internals;

internal  class CronJobDescriptor(Type type, string cronExpression, bool running, TimeZoneInfo timeZoneInfo) : ICronJobDescriptor
{
    public string Name { get; } = type.GetFullDisplayName();
    public string Schedule { get; set; } = cronExpression;
    public bool Running { get; set; } = running;
    public TimeZoneInfo TimeZoneInfo { get; set; } = timeZoneInfo;
}