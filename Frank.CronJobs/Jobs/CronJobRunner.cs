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

using System.Diagnostics.CodeAnalysis;
using Frank.CronJobs.Cron;
using Frank.CronJobs.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Frank.CronJobs.Jobs;

public sealed class CronJobRunner : IHostedService
{
    private readonly List<IDisposable> _timers = [];
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CronJobRunner> _logger;

    private CronJobRunnerOptions _options;

    public CronJobRunner(IOptionsMonitor<CronJobRunnerOptions> options,
        IServiceScopeFactory serviceScopeFactory, ILogger<CronJobRunner> logger)
    {
        _options = options?.CurrentValue ?? throw new ArgumentNullException(nameof(options));
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;

        options.OnChange(Restart);
    }

    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    private async Task RunAsync(string jobName)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var jobInstance = scope.ServiceProvider.GetRequiredKeyedService<ICronJob>(jobName);

        try
        {
            await jobInstance.RunAsync();
        }
        catch (Exception error)
        {
            _logger.LogError(error, "Job '{ServiceTypeName}' failed during running", jobName);
        }
        finally
        {
            if (jobInstance is IDisposable disposable) 
                disposable.Dispose();
            if (jobInstance is IAsyncDisposable asyncDisposable)
                await asyncDisposable.DisposeAsync();
            
            _logger.LogInformation("Job '{ServiceTypeName}' finished running", jobName);
        }
    }

    private void Start()
    {
        if (!_options.Running) return;

        var timeZone = new TimeZoneOptions(_options.TimeZone).ToTimeZoneInfo();

        foreach (var job in _options.Jobs)
        {
            if (!job.Running)
                continue;

            var cron = new CronExpression(job.Cron);

            if (!cron.IsValid)
            {
                _logger.LogWarning("Invalid cron expression for '{JobName}'", job.Name);
                continue;
            }

            var timer = new JobInterval(cron, timeZone, async () => await RunAsync(job.Name));

            _timers.Add(timer);

            timer.Run();
        }
    }

    private void Stop()
    {
        foreach (var timer in _timers)
            timer.Dispose();
        _timers.Clear();
        _logger.LogInformation("Cron job runner stopped");
    }

    private void Restart(CronJobRunnerOptions options)
    {
        _logger.LogInformation("Restarting cron job runner");
        _options = options;
        Stop();
        Start();
        _logger.LogInformation("Cron job runner restarted");
    }

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting cron job runner");
        Start();
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping cron job runner");
        Stop();
        return Task.CompletedTask;
    }
}
