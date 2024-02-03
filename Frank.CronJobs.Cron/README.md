# Frank.CronJobs.Cron

Frank.CronJobs.Cron is a .NET library that provides cron expression parsing and scheduling capabilities. Its meant as an 
internal dependency for Frank.CronJobs, but works just fine as a CronParser

## Features

- Parse cron expressions and validate syntax
- Calculate next occurrence datetime based on cron expression
- Determine if cron expression is due relative to specified datetime
- Helper methods for working with cron expressions
- Constants for common Cron Expressions

## Usage
Parse and validate cron expression

```csharp
string expression = "0 15 10 * * ?";

CronExpression cron = new CronExpression(expression);

bool isValid = cron.IsValid;
```

Calculate next occurrence

```csharp
string expression = "0 15 10 * * ?";

DateTime next = CronExpression.GetNextOccurrence(expression);
```

Check if cron is due

```csharp
string expression = "0 15 10 * * *";
DateTime dateTime = new DateTime(2023, 2, 15, 11, 0, 0);

bool isDue = CronExpression.IsDue(expression, dateTime); // true
```

Use helper methods

```csharp
// Get next occurrence from current time
DateTime next = CronHelper.GetNextOccurrence(expression);

// Get time until next occurrence 
TimeSpan timeToNext = CronHelper.GetTimeUntilNextOccurrence(expression);

// Check if due
bool isDue = CronHelper.IsDue(expression);
```

Use common cron expressions

```csharp
string everySecond = PredefinedCronExpressions.EverySecond;
string everyMinute = PredefinedCronExpressions.EveryMinute;
string everyHour = PredefinedCronExpressions.EveryHour;
string everyDay = PredefinedCronExpressions.EveryDay;
string everyWeek = PredefinedCronExpressions.EveryWeek;
string everyMonth = PredefinedCronExpressions.EveryMonth;
string everyYear = PredefinedCronExpressions.EveryYear;

string everyYearOnChristmasEve = PredefinedCronExpressions.EveryYearOn.ChristmasEve;
```

## Installation
Install the NuGet package directly from the package manager console:

```powershell
PM> Install-Package Frank.CronJobs.Cron
```

## License

Frank.CronJobs.Cron is licensed under the [MIT license](../LICENSE).

## Contributing

Contributions, except for actual bug fixes, are not welcome at this time. This is an internal dependency for Frank.CronJobs, 
and though it is a standalone library, it is not meant to be developed as such. If you have a bug fix, please submit a pull 
with a test that demonstrates the bug and the fix.

## Credits

This library is based on [CronQuery](https://github.com/marxjmoura/cronquery), which I am a contributor to. This is built on 
that code to change it in a few ways to better suit my needs for Frank.CronJobs, and make it a standalone library so the 
lightweight cron parsing can be used in other projects as well with no dependencies.