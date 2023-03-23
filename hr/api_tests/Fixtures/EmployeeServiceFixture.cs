using Hr.Api.Business.Services;
using Hr.Api.DataAccess.Repositories;
using Hr.Api.Tests.Repositories;

namespace Hr.Api.Tests.Fixtures;

public class EmployeeServiceFixture : IDisposable
{
    public IHrRepository HrTestDataRepository { get; }
    public EmployeeService EmployeeService { get; }

    public EmployeeServiceFixture()
    {
        HrTestDataRepository =
            new HrTestDataRepository();
        EmployeeService = new EmployeeService(
            HrTestDataRepository,
            new EmployeeFactory());
    }

    public void Dispose()
    {
        // clean up the setup code, if required
    }
}