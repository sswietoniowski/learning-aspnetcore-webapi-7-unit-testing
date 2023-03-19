using Hr.Api.Business;
using Hr.Api.DataAccess.Entities;

namespace Hr.Api.Services;

public class EmployeeService : IEmployeeService
{
    public event EventHandler<EmployeeIsAbsentEventArgs>? EmployeeIsAbsent;

    public Task AttendCourseAsync(InternalEmployee employee, Course attendedCourse)
    {
        throw new NotImplementedException();
    }

    public ExternalEmployee CreateExternalEmployee(string firstName, string lastName, string company)
    {
        throw new NotImplementedException();
    }

    public InternalEmployee CreateInternalEmployee(string firstName, string lastName)
    {
        throw new NotImplementedException();
    }

    public Task CreateInternalEmployeeAsync(InternalEmployee internalEmployee)
    {
        throw new NotImplementedException();
    }

    public Task<InternalEmployee> CreateInternalEmployeeAsync(string firstName, string lastName)
    {
        throw new NotImplementedException();
    }

    public InternalEmployee? GetInternalEmployeeById(Guid employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<InternalEmployee?> GetInternalEmployeeByIdAsync(Guid employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<InternalEmployee>> GetInternalEmployeesAsync()
    {
        throw new NotImplementedException();
    }

    public Task GiveMinimumRaiseAsync(InternalEmployee employee)
    {
        throw new NotImplementedException();
    }

    public Task GiveRaiseAsync(InternalEmployee employee, int raise)
    {
        throw new NotImplementedException();
    }

    public void NotifyOfAbsence(Employee employee)
    {
        throw new NotImplementedException();
    }
}