namespace Frank.CronJobs.Cron;

public class PredefinedCronExpressions
{
    public static PredefinedCronExpressions Instance { get; } = new PredefinedCronExpressions();

    public const string EverySecond = "* * * * * *";
    
    public const string EveryFiveSeconds = "*/5 * * * * *";
    
    public const string EveryMinute = "0 * * * * *";

    public const string EveryFiveMinutes = "0 */5 * * * *";
    
    public const string EveryHour = "0 0 * * * *";

    public const string EveryDay = "0 0 0 * * *";
    
    public const string EveryWeekday = "0 0 0 * * 1-5";
    
    public const string EverySunday = "0 0 0 * * 0";
}