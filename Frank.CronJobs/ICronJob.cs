namespace Frank.CronJobs;

public interface ICronJob
{
    Task RunAsync(CancellationToken cancellationToken);
}
