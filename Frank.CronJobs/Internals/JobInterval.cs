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

using System.Reactive.Linq;
using Frank.CronJobs.Cron;

namespace Frank.CronJobs.Internals;

internal sealed class JobInterval(CronExpression cron, TimeZoneInfo timezone, Func<Task> work) : IDisposable
{
    private readonly CronExpression _cron = cron ?? throw new ArgumentNullException(nameof(cron));
    private readonly TimeZoneInfo _timezone = timezone ?? throw new ArgumentNullException(nameof(timezone));
    private readonly Func<Task> _work = work ?? throw new ArgumentNullException(nameof(work));

    private IDisposable _subscription = null!;

    public void Dispose() => _subscription.Dispose();

    public void Run()
    {
        var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timezone);
        var nextTime = _cron.Next(now);

        if (nextTime == DateTime.MinValue)
            return;

        var interval = nextTime - now;

        _subscription = Observable.Timer(interval)
            .Select(tick => Observable.FromAsync(_work))
            .Concat()
            .Subscribe(
                onNext: tick => { /* noop */ },
                onCompleted: Run
            );
    }
}