namespace Frank.CronJobs.Cron;

public static class CronHelper
{
    public static DateTime GetNextOccurrence(string cronExpression) 
        => new CronExpression(cronExpression).Next(DateTime.UtcNow);

    public static DateTime GetNextOccurrence(CronExpression cronExpression) 
        => cronExpression.Next(DateTime.UtcNow);

    public static DateTime GetNextOccurrence(CronExpression cronExpression, DateTime fromUtc)
        => cronExpression.Next(fromUtc);

    public static DateTime GetNextOccurrence(string cronExpression, DateTime fromUtc)
        => new CronExpression(cronExpression).Next(fromUtc);
    
    public static TimeSpan GetTimeUntilNextOccurrence(string cronExpression)
        => GetNextOccurrence(cronExpression) - DateTime.UtcNow;
    
    public static TimeSpan GetTimeUntilNextOccurrence(CronExpression cronExpression)
        => GetNextOccurrence(cronExpression) - DateTime.UtcNow;
    
    public static TimeSpan GetTimeUntilNextOccurrence(CronExpression cronExpression, DateTime fromUtc)
        => GetNextOccurrence(cronExpression, fromUtc) - fromUtc;
    
    public static TimeSpan GetTimeUntilNextOccurrence(string cronExpression, DateTime fromUtc)
        => GetNextOccurrence(cronExpression, fromUtc) - fromUtc;
    
    public static bool IsDue(string cronExpression)
        => GetTimeUntilNextOccurrence(cronExpression) == TimeSpan.Zero;
    
    public static bool IsDue(CronExpression cronExpression)
        => GetTimeUntilNextOccurrence(cronExpression) == TimeSpan.Zero;
    
    public static bool IsDue(CronExpression cronExpression, DateTime fromUtc)
        => GetTimeUntilNextOccurrence(cronExpression, fromUtc) == TimeSpan.Zero;
    
    public static bool IsDue(string cronExpression, DateTime fromUtc)
        => GetTimeUntilNextOccurrence(cronExpression, fromUtc) == TimeSpan.Zero;

    public static bool IsValid(string cronExpression)
        => new CronExpression(cronExpression).IsValid;

    public static PredefinedCronExpressions Predefined => PredefinedCronExpressions.Instance;
}