using Hr.Api.DataAccess;
using Hr.Api.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hr.Api.Tests;

public class ServiceCollectionTests
{
    [Fact]
    public void RegisterDataServices_Execute_DataServicesAreRegistered()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string> {
                    {"ConnectionStrings:EmployeeManagementDB", "AnyValueWillDo"}})
            .Build();

        // Act
        serviceCollection.AddDbContext<HrDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("EmployeeManagementDB")));
        serviceCollection.AddScoped<IHrRepository, HrRepository>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Assert
        Assert.NotNull(
            serviceProvider.GetService<IHrRepository>());
        Assert.IsType<HrRepository>(
            serviceProvider.GetService<IHrRepository>());
    }
}