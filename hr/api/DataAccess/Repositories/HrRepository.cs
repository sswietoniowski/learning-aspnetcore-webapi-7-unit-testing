using Hr.Api.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hr.Api.DataAccess.Repositories;

public class HrRepository : IHrRepository
{
    private readonly HrDbContext _context;

    public HrRepository(HrDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<InternalEmployee>> GetInternalEmployeesAsync() =>
        await _context.InternalEmployees
            .Include(e => e.AttendedCourses)
            .ToListAsync();

    public InternalEmployee? GetInternalEmployeeById(Guid employeeId) =>
        _context.InternalEmployees
            .Include(e => e.AttendedCourses)
            .FirstOrDefault(e => e.Id == employeeId);

    public async Task<InternalEmployee?> GetInternalEmployeeByIdAsync(Guid employeeId) =>
        await _context.InternalEmployees
            .Include(e => e.AttendedCourses)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

    public void CreateInternalEmployee(InternalEmployee internalEmployee)
    {
        _context.InternalEmployees.Add(internalEmployee);
    }

    public Course? GetCourseById(Guid courseId) =>
        _context.Courses.FirstOrDefault(c => c.Id == courseId);

    public async Task<Course?> GetCourseByIdAsync(Guid courseId) =>
        await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

    public List<Course> GetCoursesByIds(params Guid[] courseIds) =>
        _context.Courses.Where(c => courseIds.Contains(c.Id)).ToList();

    public async Task<List<Course>> GetCoursesByIdsAsync(params Guid[] courseIds) =>
        await _context.Courses.Where(c => courseIds.Contains(c.Id)).ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}