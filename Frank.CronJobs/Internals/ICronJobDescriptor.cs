namespace Frank.CronJobs.Internals;

internal interface ICronJobDescriptor
{
    string Name { get; }
    string Schedule { get; set; }
    bool Running { get; set; }
    TimeZoneInfo TimeZoneInfo { get; set; }
}