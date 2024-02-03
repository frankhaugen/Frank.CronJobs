namespace Frank.CronJobs.Cron;

/// <summary>
/// Provides helper methods to work with cron expressions.
/// </summary>
public static class CronHelper
{
    /// <summary>
    /// Parses a cron expression and returns a CronExpression object.
    /// </summary>
    /// <param name="expression">The cron expression to parse.</param>
    /// <returns>A CronExpression object representing the parsed cron expression.</returns>
    public static CronExpression Parse(string expression) => new(expression);

    /// <summary>
    /// Tries to parse the provided cron expression and creates a CronExpression object if the expression is valid.
    /// </summary>
    /// <param name="expression">The cron expression to parse.</param>
    /// <param name="cronExpression">When this method returns, contains the CronExpression object created from the parsed expression if the expression is valid; otherwise, contains null.</param>
    /// <returns>Returns true if the cron expression was successfully parsed and created into a CronExpression object; otherwise, returns false.</returns>
    public static bool TryParse(string expression, out CronExpression? cronExpression)
    {
        if (IsValid(expression))
        {
            cronExpression = new CronExpression(expression);
            return true;
        }
        cronExpression = null;
        return false;
    }

    /// <summary>
    /// Calculates the next occurrence of a cron expression based on the current UTC time.
    /// </summary>
    /// <param name="cronExpression">The cron expression string to be evaluated.</param>
    /// <returns>A DateTime object representing the next occurrence of the cron expression.</returns>
    public static DateTime GetNextOccurrence(string cronExpression) 
        => new CronExpression(cronExpression).Next(DateTime.UtcNow);

    /// <summary>
    /// Returns the next occurrence of a given cron expression.
    /// </summary>
    /// <param name="cronExpression">The cron expression used to calculate the next occurrence.</param>
    /// <returns>The next occurrence as a DateTime object.</returns>
    public static DateTime GetNextOccurrence(CronExpression cronExpression) 
        => cronExpression.Next(DateTime.UtcNow);

    /// <summary>
    /// Calculates the next occurrence of a scheduled event based on the given cron expression and starting date/time.
    /// </summary>
    /// <param name="cronExpression">The cron expression specifying the schedule.</param>
    /// <param name="fromUtc">The starting date/time to calculate the next occurrence from.</param>
    /// <returns>The next occurrence of the scheduled event as a DateTime object.</returns>
    public static DateTime GetNextOccurrence(CronExpression cronExpression, DateTime fromUtc)
        => cronExpression.Next(fromUtc);

    /// <summary>
    /// Returns the next occurrence of the specified cron expression after the given DateTime.
    /// </summary>
    /// <param name="cronExpression">The cron expression to evaluate.</param>
    /// <param name="fromUtc">The DateTime to start searching for the next occurrence.</param>
    /// <returns>The DateTime of the next occurrence based on the specified cron expression and starting DateTime.</returns>
    public static DateTime GetNextOccurrence(string cronExpression, DateTime fromUtc)
        => new CronExpression(cronExpression).Next(fromUtc);

    /// <summary>
    /// Calculates the time difference between the current UTC time and the next occurrence of a cron expression.
    /// </summary>
    /// <param name="cronExpression">The cron expression specifying the schedule pattern.</param>
    /// <returns>The time interval until the next occurrence.</returns>
    public static TimeSpan GetTimeUntilNextOccurrence(string cronExpression)
        => GetNextOccurrence(cronExpression) - DateTime.UtcNow;

    /// <summary>
    /// Returns the time duration until the next occurrence specified by the cron expression.
    /// </summary>
    /// <param name="cronExpression">The cron expression defining the occurrence pattern.</param>
    /// <returns>The time duration until the next occurrence.</returns>
    public static TimeSpan GetTimeUntilNextOccurrence(CronExpression cronExpression)
        => GetNextOccurrence(cronExpression) - DateTime.UtcNow;

    /// <summary>
    /// Calculates the time remaining until the next occurrence of the provided cron expression, starting from the specified UTC time.
    /// </summary>
    /// <param name="cronExpression">The cron expression used to determine occurrence times.</param>
    /// <param name="fromUtc">The starting date and time in UTC.</param>
    /// <returns>The time remaining until the next occurrence as a TimeSpan.</returns>
    public static TimeSpan GetTimeUntilNextOccurrence(CronExpression cronExpression, DateTime fromUtc)
        => GetNextOccurrence(cronExpression, fromUtc) - fromUtc;

    /// <summary>
    /// Calculates the time remaining until the next occurrence of the cron expression
    /// from the given DateTime in UTC timezone.
    /// </summary>
    /// <param name="cronExpression">The cron expression representing the schedule.</param>
    /// <param name="fromUtc">The DateTime from which to start calculating.</param>
    /// <returns>
    /// A TimeSpan representing the time remaining until the next occurrence of the cron expression.
    /// </returns>
    public static TimeSpan GetTimeUntilNextOccurrence(string cronExpression, DateTime fromUtc)
        => GetNextOccurrence(cronExpression, fromUtc) - fromUtc;

    /// <summary>
    /// Checks if the next occurrence of a recurring event specified by the cron expression is due.
    /// </summary>
    /// <param name="cronExpression">The cron expression representing the recurring event schedule.</param>
    /// <returns>
    /// True if the next occurrence of the recurring event is due; otherwise, false.
    /// </returns>
    public static bool IsDue(string cronExpression)
        => GetTimeUntilNextOccurrence(cronExpression) == TimeSpan.Zero;

    /// <summary>
    /// Determines if the next occurrence of a specified cron expression is due.
    /// </summary>
    /// <param name="cronExpression">The cron expression to evaluate.</param>
    /// <returns>True if the next occurrence is due; otherwise, false.</returns>
    public static bool IsDue(CronExpression cronExpression)
        => GetTimeUntilNextOccurrence(cronExpression) == TimeSpan.Zero;

    /// <summary>
    /// Determines whether the given <paramref name="cronExpression"/> is due at the specified <paramref name="fromUtc"/> time.
    /// </summary>
    /// <param name="cronExpression">The cron expression to evaluate.</param>
    /// <param name="fromUtc">The reference time in UTC.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="cronExpression"/> is due at the specified <paramref name="fromUtc"/> time;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDue(CronExpression cronExpression, DateTime fromUtc)
        => GetTimeUntilNextOccurrence(cronExpression, fromUtc) == TimeSpan.Zero;

    /// <summary>
    /// Determines if the specified cron expression is due relative to the provided starting date and time.
    /// </summary>
    /// <param name="cronExpression">The cron expression to evaluate.</param>
    /// <param name="fromUtc">The starting date and time in UTC format.</param>
    /// <returns>
    /// True if the cron expression is due; otherwise, false.
    /// </returns>
    public static bool IsDue(string cronExpression, DateTime fromUtc)
        => GetTimeUntilNextOccurrence(cronExpression, fromUtc) == TimeSpan.Zero;

    /// <summary>
    /// Checks if the provided cron expression is valid.
    /// </summary>
    /// <param name="cronExpression">The cron expression to validate.</param>
    /// <returns>Returns true if the cron expression is valid; otherwise, returns false.</returns>
    public static bool IsValid(string cronExpression)
        => new CronExpression(cronExpression).IsValid;

    /// <summary>
    /// Provides access to a singleton instance of PredefinedCronExpressions.
    /// </summary>
    public static PredefinedCronExpressions Predefined => PredefinedCronExpressions.Instance;
}