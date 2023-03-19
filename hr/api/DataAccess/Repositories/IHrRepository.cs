using Hr.Api.DataAccess.Entities;

namespace Hr.Api.DataAccess.Repositories;

public interface IHrRepository
{
    Task<IEnumerable<InternalEmployee>> GetInternalEmployeesAsync();
    InternalEmployee? GetInternalEmployeeById(Guid employeeId);
    Task<InternalEmployee?> GetInternalEmployeeByIdAsync(Guid employeeId);
    void CreateInternalEmployee(InternalEmployee internalEmployee);
    List<Course> GetCoursesByIds(params Guid[] courseIds);
    Task<List<Course>> GetCoursesByIdsAsync(params Guid[] courseIds);
    Course? GetCourseById(Guid courseId);
    Task<Course?> GetCourseByIdAsync(Guid courseId);
    Task SaveChangesAsync();
}
