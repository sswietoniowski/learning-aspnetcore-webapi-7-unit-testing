using Hr.Api.Business.Services;
using Hr.Api.DataAccess.Repositories;
using Hr.Api.Tests.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Hr.Api.Tests.Fixtures;

public class EmployeeServiceWithAspNetCoreDIFixture : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public IHrRepository HrTestDataRepository
    {
        get
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _serviceProvider.GetService<IHrRepository>();
#pragma warning restore CS8603 // Possible null reference return.
        }
    }

    public IEmployeeService EmployeeService
    {
        get
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _serviceProvider.GetService<IEmployeeService>();
#pragma warning restore CS8603 // Possible null reference return.
        }
    }


    public EmployeeServiceWithAspNetCoreDIFixture()
    {
        var services = new ServiceCollection();
        services.AddScoped<EmployeeFactory>();
        services.AddScoped<IHrRepository, HrTestDataRepository>();
        services.AddScoped<IEmployeeService, EmployeeService>();

        // build provider
        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        // clean up the setup code, if required
    }
}