namespace Frank.CronJobs.Cron;

/// <summary>
/// Represents a collection of predefined cron expressions.
/// </summary>
public class PredefinedCronExpressions
{
    private PredefinedCronExpressions() { } // Prevent instantiation

    /// <summary>
    /// Gets the singleton instance of the PredefinedCronExpressions class.
    /// </summary>
    /// <value>
    /// The singleton instance of the PredefinedCronExpressions class.
    /// </value>
    public static PredefinedCronExpressions Instance { get; } = new();

    /// <summary>
    /// Represents a constant string value that represents every second.
    /// </summary>
    public const string EverySecond = "* * * * * *";

    /// <summary>
    /// Represents the cron expression for executing a task every five seconds.
    /// </summary>
    public const string EveryFiveSeconds = "*/5 * * * * *";

    /// <summary>
    /// Represents a string constant that specifies a cron expression for every minute.
    /// </summary>
    public const string EveryMinute = "0 * * * * *";

    /// <summary>
    /// Represents the cron expression for running a task every five minutes.
    /// </summary>
    public const string EveryFiveMinutes = "0 */5 * * * *";

    /// <summary>
    /// Represents a cron expression that runs every hour.
    /// </summary>
    public const string EveryHour = "0 0 * * * *";

    /// <summary>
    /// Represents a constant string that defines a cron expression for running a task every day.
    /// The expression "0 0 0 * * *" corresponds to the start of every day.
    /// </summary>
    public const string EveryDay = "0 0 0 * * *";

    /// <summary>
    /// Represents the CRON expression for executing a recurring task every weekday.
    /// </summary>
    public const string EveryWeekday = "0 0 0 * * 1-5";

    /// <summary>
    /// Represents a constant variable that holds the cron expression for every Sunday.
    /// </summary>
    public const string EverySunday = "0 0 0 * * 0";

    /// <summary>
    /// Represents a constant that specifies a cron expression for every Monday at midnight.
    /// </summary>
    public const string EveryMonday = "0 0 0 * * 1";

    /// <summary>
    /// Represents a schedule expression that occurs every Tuesday.
    /// </summary>
    public const string EveryTuesday = "0 0 0 * * 2";

    /// <summary>
    /// Represents the cron expression for every Wednesday.
    /// </summary>
    public const string EveryWednesday = "0 0 0 * * 3";

    /// <summary>
    /// Represents a cron expression for running a task on every Thursday.
    /// </summary>
    public const string EveryThursday = "0 0 0 * * 4";

    /// <summary>
    /// Represents a constant variable that specifies the schedule for every Friday.
    /// </summary>
    public const string EveryFriday = "0 0 0 * * 5";

    /// <summary>
    /// Represents a constant string that represents a cron expression for every Saturday.
    /// </summary>
    public const string EverySaturday = "0 0 0 * * 6";

    /// <summary>
    /// Represents a constant string that defines a cron expression for every weekend day.
    /// The expression "0 0 0 * * 0,6" means to execute the specified task at 00:00 (midnight)
    /// on Sunday (0) and Saturday (6) of every month.
    /// </summary>
    public const string EveryWeekendDay = "0 0 0 * * 0,6";

    /// <summary>
    /// Represents the cron expression for scheduling a task to run every day at noon.
    /// </summary>
    public const string EveryDayAtNoon = "0 0 12 * * *";

    /// <summary>
    /// Represents a schedule for a recurring event that occurs every month.
    /// The schedule is defined by a Cron expression.
    /// </summary>
    public const string EveryMonth = "0 0 0 1 * *";

    /// <summary>
    /// Represents a cron expression that triggers an event at
    /// midnight on the last day of every month.
    /// </summary>
    public const string EveryLastDayOfMonth = "0 0 0 L * *";

    /// <summary>
    /// Represents a cron expression that triggers every year on January 1st at midnight.
    /// </summary>
    public const string EveryYear = "0 0 0 1 1 *";

    /// <summary>
    /// The EveryYearOn class contains constants representing specific dates that occur every year.
    /// </summary>
    public static class EveryYearOn
    {
        /// <summary>
        /// Represents a schedule pattern for a leap day.
        /// </summary>
        public const string LeapDay = "0 0 0 29 2 *";

        /// <summary>
        /// Represents the cron expression for New Year's Day.
        /// </summary>
        public const string NewYearsDay = "0 0 0 1 1 *";

        /// <summary>
        /// Holds the cron expression for Christmas Day.
        /// </summary>
        public const string ChristmasDay = "0 0 0 25 12 *";

        /// <summary>
        /// Holds the cron expression for Christmas Eve.
        /// </summary>
        public const string ChristmasEve = "0 0 0 24 12 *";

        /// <summary>
        /// Represents the cron expression for Halloween.
        /// </summary>
        public const string Halloween = "0 0 0 31 10 *";
        
        /// <summary>
        /// Represents the cron expression for Thanksgiving.
        /// </summary>
        /// <remarks>
        /// Thanksgiving is the fourth Thursday of November.
        /// </remarks>
        public const string Thanksgiving = "0 0 0 * 11 4";
    }
}