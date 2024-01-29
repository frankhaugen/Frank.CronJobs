namespace Frank.CronJobs.Internals;

/// <summary>
/// Represents a collection of CronJobOptions.
/// </summary>
internal sealed class CronJobOptionsCollection : List<CronJobOptions>
{
    public void Replace(CronJobOptions cronJobOptions)
    {
        if (this.Any(s => s.Name == cronJobOptions.Name))
        {
            var index = this.FindIndex(s => s.Name == cronJobOptions.Name);
            this[index] = cronJobOptions;
        }
        else
        {
            Add(cronJobOptions);
        }
    }
    
    
    public void Replace(IEnumerable<CronJobOptions> cronJobOptions)
    {
        foreach (var job in cronJobOptions) 
            Replace(job);
    }
    
    public new void Add(CronJobOptions cronJobOptions)
    {
        if (this.Any(s => s.Name == cronJobOptions.Name))
            throw new ArgumentException($"CronJobOptions already exists. Name was: {cronJobOptions.Name}");
        
        base.Add(cronJobOptions);
    }
    
    public CronJobOptions Find(string jobName) => Find(service => service.Name == jobName) ?? throw new InvalidOperationException($"Cron job with name '{jobName}' not found.");
}