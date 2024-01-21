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
```c#
using Frank.CronJobs;

var cron = new CronJob("* * * * *");
var next = cron.GetNextOccurrence(DateTime.Now);
```

