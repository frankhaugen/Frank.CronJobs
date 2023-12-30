/*
 * Based on "CronQuery", which is licensed under the MIT license
 */

namespace Frank.CronJobs.Jobs;

public interface ICronJob
{
    Task RunAsync();
}
