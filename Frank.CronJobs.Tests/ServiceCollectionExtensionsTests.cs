using FluentAssertions;
using Frank.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Frank.CronJobs.Tests;

public class ServiceCollectionExtensionsTests
{
    [Test]
    public async Task AddCronJob_WithValidCronExpression_AddsCronJobToServiceCollection()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var cronExpression = "0 0 * * * *";

        // Act
        serviceCollection.AddCronJob<MockCronJob>(cronExpression);

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var cronJob = serviceProvider.GetRequiredKeyedService<ICronJob>(typeof(MockCronJob).GetFullDisplayName());
        await Assert.That(cronJob).IsTypeOf<MockCronJob>();
    }

    [Test]
    public void AddCronJob_WithInvalidCronExpression_ThrowsInvalidCronExpressionException()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var cronExpression = "invalid";
        
        // Act
        Action act = () => serviceCollection.AddCronJob<MockCronJob>(cronExpression);
        
        // Assert
        act.Should().Throw<InvalidCronExpressionException>();
    }
    
    public class MockCronJob : ICronJob
    {
        public Task RunAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}