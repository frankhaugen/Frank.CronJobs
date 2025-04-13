using System.Diagnostics.CodeAnalysis;
using Frank.CronJobs.Cron;

namespace Frank.CronJobs.Internals;

internal sealed class JobInterval(ICronJobDescriptor descriptor, Func<Task> work, CancellationToken cancellationToken) : IDisposable
{
    // The registration should be stored and disposed
    private CancellationTokenRegistration _registration;
    private Timer? _timer;

    public void Dispose()
    {
        _timer?.Dispose();
        _registration.Dispose();
    }
    
    [SuppressMessage("ReSharper", "TailRecursiveCall")]
    public void Run()
    {
        // Dispose previous registration
        _registration.Dispose();
        
        var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, descriptor.TimeZoneInfo);
        var cronExpression = new CronExpression(descriptor.Schedule);
        var nextTime = cronExpression.Next(now);

        // If no next time is found, do not schedule further executions
        if (nextTime == DateTime.MinValue)
            return;

        var interval = nextTime - now;
        if (interval <= TimeSpan.Zero)
        {
            // If the calculated interval is in the past, schedule immediately for the next possible interval
            Run();
            return;
        }

        _timer = new Timer(async _ =>
        {
            try
            {
                await work();
            }
            catch
            {
                // Ignore exceptions
            }
            
            // Reschedule the next run after the current work is completed
            Run();
        }, null, interval, Timeout.InfiniteTimeSpan); // Timeout.InfiniteTimeSpan prevents periodic signalling
        
        _registration = cancellationToken.Register(() => _timer?.Dispose());
    }

    public void Refresh(ICronJobDescriptor cronJobDescriptor)
    {
        _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        descriptor = cronJobDescriptor;
        Run();
    }
}
