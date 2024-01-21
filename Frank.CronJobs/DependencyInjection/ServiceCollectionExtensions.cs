/*
 * MIT License
 *
 * Copyright (c) 2018 Marx J. Moura
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Frank.CronJobs.Jobs;
using Frank.CronJobs.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frank.CronJobs.DependencyInjection;

/// <summary>
/// Provides extension methods for the IServiceCollection class.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds cron jobs to the specified service collection using the provided configuration and builder action.
    /// </summary>
    /// <param name="services">The service collection to add the cron jobs to.</param>
    /// <param name="configuration">The configuration containing the cron job options.</param>
    /// <param name="builderAction">An action to configure the cron job builder.</param>
    public static void AddCronJobs(this IServiceCollection services, IConfiguration configuration, Action<ICronJobsBuilder> builderAction)
    {
        var options = new CronJobRunnerOptions();
        configuration.Bind(nameof(CronJobRunnerOptions), options);
        var tempOptionsCollection = new CronJobOptionsCollection();
        tempOptionsCollection.AddRange(options.Jobs);
        
        var builder = new CronJobsBuilder(services, options);
        builderAction(builder);
        
        options.Jobs.Replace(tempOptionsCollection);
        
        services.Configure<CronJobRunnerOptions>(c =>
        {
            c.Jobs.Replace(options.Jobs);
            c.TimeZone = options.TimeZone;
            c.Running = options.Running;
        });
        
        services.AddHostedService<CronJobRunner>();
    }
}