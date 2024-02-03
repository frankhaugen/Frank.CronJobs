namespace Frank.CronJobs;

public class InvalidCronExpressionException(string cronExpression) : Exception
{
    public string InvalidCronExpression { get; private set; } = cronExpression;
}