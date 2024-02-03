# Frank.CronJobs
This is based on CronQuery, which I am a contributor to. This is built on that code to extend its functionality in experimental ways

___
[![GitHub License](https://img.shields.io/github/license/frankhaugen/Frank.CronJobs)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Frank.CronJobs.svg)](https://www.nuget.org/packages/Frank.CronJobs)
[![NuGet](https://img.shields.io/nuget/dt/Frank.CronJobs.svg)](https://www.nuget.org/packages/Frank.CronJobs)

![GitHub contributors](https://img.shields.io/github/contributors/frankhaugen/Frank.CronJobs)
![GitHub Release Date - Published_At](https://img.shields.io/github/release-date/frankhaugen/Frank.CronJobs)
![GitHub last commit](https://img.shields.io/github/last-commit/frankhaugen/Frank.CronJobs)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/frankhaugen/Frank.CronJobs)
![GitHub pull requests](https://img.shields.io/github/issues-pr/frankhaugen/Frank.CronJobs)
![GitHub issues](https://img.shields.io/github/issues/frankhaugen/Frank.CronJobs)
![GitHub closed issues](https://img.shields.io/github/issues-closed/frankhaugen/Frank.CronJobs)
___

## Usage

This library is designed to be used with the `Microsoft.Extensions.DependencyInjection` library. It is designed to be used 
list this in your `Program.cs` file:

```c#
using Frank.CronJobs;

var services = new ServiceCollection();
services.AddCronJob<MyJob>("0 15 10 * * ?");

var serviceProvider = services.BuildServiceProvider();

await serviceProvider.StartAsync();
```

This will start the job at 10:15 AM every day. The `MyJob` class should implement the `ICronJob` interface, and the
`ExecuteAsync` method should be implemented. This method will be called every time the cron expression is satisfied.

If you want to stop or modify the job, you can use the `IScheuleMaintainer` interface to stop, start, or modify any job at 
runtime like this:

```c#
var maintainer = serviceProvider.GetRequiredService<IScheduleMaintainer>();
maintainer.Stop<MyJob>();
```

This will stop the job from running. You can also start it again by calling `Start<MyJob>()`. You can also modify the 
schedule and timezones of the job by calling `SetSchedule<MyJob>("0 15 10 * * ?")` and `SetTimeZone<MyJob>
("America/New_York")` respectively.

The job runner is added as a hosted service, so it will run as long as the application is running.

The scenario for editing the schedule at runtime is meant to make it flexible for the user to change the schedule of the
job without having to restart the application, or specify the schedule in a configuration file, and so anything that can 
have the opportunity y to change the schedule at runtime. A service that react to IOptions changes, for example, can 
look like this:

```c#
public class MyService
{
    private readonly IOptionsMonitor<MyOptions> _optionsMonitor;
    private readonly IScheduleMaintainer _scheduleMaintainer;

    public MyService(IOptionsMonitor<MyOptions> optionsMonitor, IScheduleMaintainer scheduleMaintainer)
    {
        _optionsMonitor = optionsMonitor;
        _scheduleMaintainer = scheduleMaintainer;
        _optionsMonitor.OnChange(OnOptionsChange);
    }

    private void OnOptionsChange(MyOptions options)
    {
        _scheduleMaintainer.SetSchedule<MyJob>(options.CronExpression);
        // etc.
    }
}
```


## Installation

Install the NuGet package directly from the package manager console:

```powershell
PM> Install-Package Frank.CronJobs
```

## License

Frank.CronJobs is licensed under the [MIT license](LICENSE).

## Contributing

Contributions are welcome, please submit a pull request after you create an issue if you have any ideas or find any 
bugs. Major changes should be discussed in an issue before submitting a pull request. Also, no new dependencies unless 
discussed and agreed upon in an issue first.

## Credits

This library is based on [CronQuery](https://github.com/marxjmoura/cronquery), which I am a contributor to. This is built on 
that code for the basic cron functionality, and some patterns and ideas are borrowed from that project.