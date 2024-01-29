/*
 * Based on "CronQuery", which is licensed under the MIT license
 */

namespace Frank.CronJobs;

public interface ICronJob
{
    Task RunAsync();
}
