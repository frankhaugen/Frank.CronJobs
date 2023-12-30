using Frank.CronJobs.Cron;
using Frank.CronJobs.Jobs;
using Frank.CronJobs.Options;
using Frank.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Frank.CronJobs.DependencyInjection;

internal sealed class CronJobsBuilder(IServiceCollection services, CronJobRunnerOptions options) : ICronJobsBuilder
{
    public ICronJobsBuilder AddCronJob<T>() where T : class, ICronJob => AddCronJob<T>("* * * * * *");

    public ICronJobsBuilder AddCronJob<T>(string cron) where T : class, ICronJob => AddCronJob<T>(new CronExpression(cron));
    
    public ICronJobsBuilder AddCronJob<T>(CronExpression cronExpression) where T : class, ICronJob => AddCronJob<T>(jobOptions =>
    {
        jobOptions.Cron = cronExpression.ToString();
        jobOptions.Running = true;
    });
    
    public ICronJobsBuilder AddCronJob<T>(Action<CronJobOptions> configure) where T : class, ICronJob
    {
        var jobOptions = new CronJobOptions();
        configure(jobOptions);
        return AddCronJob<T>(jobOptions);
    }

    public ICronJobsBuilder AddCronJob<T>(CronJobOptions jobOptions) where T : class, ICronJob
    {
        var serviceName = typeof(T).GetDisplayName();
        var service = new ServiceDescriptor(typeof(ICronJob), serviceName, typeof(T), ServiceLifetime.Singleton);
        jobOptions.Name = serviceName;
        
        options.Jobs.Add(jobOptions);
        services.Add(service);
        
        return this;
    }


}