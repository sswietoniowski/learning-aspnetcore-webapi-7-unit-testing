using Hr.Api.Business.Events;
using Hr.Api.DataAccess.Entities;

namespace Hr.Api.Business.Services;

public interface IEmployeeService
{
    event EventHandler<EmployeeIsAbsentEventArgs>? EmployeeIsAbsent;

    InternalEmployee CreateInternalEmployee(string firstName,
        string lastName);
    Task CreateInternalEmployeeAsync(InternalEmployee internalEmployee);
    ExternalEmployee CreateExternalEmployee(string firstName,
        string lastName, string company);
    Task<InternalEmployee> CreateInternalEmployeeAsync(string firstName,
        string lastName);
    Task AttendCourseAsync(InternalEmployee employee, Course attendedCourse);
    InternalEmployee? GetInternalEmployeeById(Guid employeeId);
    Task<InternalEmployee?> GetInternalEmployeeByIdAsync(Guid employeeId);
    Task<IEnumerable<InternalEmployee>> GetInternalEmployeesAsync();
    Task GiveMinimumRaiseAsync(InternalEmployee employee);
    Task GiveRaiseAsync(InternalEmployee employee, int raise);
    void NotifyOfAbsence(Employee employee);
}