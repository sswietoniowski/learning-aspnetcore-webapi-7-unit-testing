using Hr.Api.Business.Events;
using Hr.Api.Business.Exceptions;
using Hr.Api.DataAccess.Entities;
using Hr.Api.DataAccess.Repositories;

namespace Hr.Api.Business.Services;

public class EmployeeService : IEmployeeService
{
    // Ids of obligatory courses: "Company Introduction" and "Respecting Your Colleagues" 
    private readonly Guid[] _obligatoryCourseIds = 
    {
        Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
        Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e") 
    };
    private readonly IHrRepository _repository;
    private readonly EmployeeFactory _employeeFactory;

    public EmployeeService(IHrRepository hrRepository, EmployeeFactory employeeFactory)
    {
        _repository = hrRepository ?? throw new ArgumentNullException(nameof(hrRepository));
        _employeeFactory = employeeFactory ?? throw new ArgumentNullException(nameof(employeeFactory));
    }

    public event EventHandler<EmployeeIsAbsentEventArgs>? EmployeeIsAbsent;

    public async Task AttendCourseAsync(InternalEmployee employee, Course attendedCourse)
    {
        var alreadyAttendedCourse = employee.AttendedCourses
            .Any(c => c.Id == attendedCourse.Id);

        if (alreadyAttendedCourse)
        {
            return;
        }

        // add course 
        employee.AttendedCourses.Add(attendedCourse);

        // save changes 
        await _repository.SaveChangesAsync();

        // execute business logic: when a course is attended, 
        // the suggested bonus must be recalculated
        employee.SuggestedBonus = employee.YearsOfService
            * employee.AttendedCourses.Count * 100;
    }

    public ExternalEmployee CreateExternalEmployee(string firstName, string lastName, string company)
    {
        // create a new external employee with default values 
        var employee = (ExternalEmployee)_employeeFactory.CreateEmployee(
            firstName, lastName, company, true);

        // no obligatory courses for external employees, return it
        return employee;
    }

    public InternalEmployee CreateInternalEmployee(string firstName, string lastName)
    {
        // use the factory to create the object 
        var employee = (InternalEmployee)_employeeFactory.CreateEmployee(firstName, lastName);

        // apply business logic 

        // add obligatory courses attended by all new employees
        // during vetting process

        // get those courses  
        var obligatoryCourses = _repository.GetCoursesByIds(_obligatoryCourseIds);

        // add them for this employee
        foreach (var obligatoryCourse in obligatoryCourses)
        {
            employee.AttendedCourses.Add(obligatoryCourse);
        }

        // calculate the suggested bonus
        employee.SuggestedBonus = CalculateSuggestedBonus(employee);

        return employee;
    }

    public async Task CreateInternalEmployeeAsync(InternalEmployee internalEmployee)
    {
        _repository.CreateInternalEmployee(internalEmployee);
        await _repository.SaveChangesAsync();
    }

    public async Task<InternalEmployee> CreateInternalEmployeeAsync(string firstName, string lastName)
    {
        // use the factory to create the object 
        var employee = (InternalEmployee)_employeeFactory.CreateEmployee(firstName, lastName);

        // apply business logic 
    
        // add obligatory courses attended by all new employees
        // during vetting process

        // get those courses  
        var obligatoryCourses = await _repository.GetCoursesByIdsAsync(_obligatoryCourseIds);

        // add them for this employee
        foreach (var obligatoryCourse in obligatoryCourses)
        {
            employee.AttendedCourses.Add(obligatoryCourse);
        }

        // calculate the suggested bonus
        employee.SuggestedBonus = CalculateSuggestedBonus(employee);

        return employee;
    }

    private int CalculateSuggestedBonus(InternalEmployee employee)
    {
        if (employee.YearsOfService == 0)
        {
            return employee.AttendedCourses.Count * 100;
        }
        else
        {
            return employee.YearsOfService
                * employee.AttendedCourses.Count * 100;
        }
    }

    public InternalEmployee? GetInternalEmployeeById(Guid employeeId)
    {
        var employee = _repository.GetInternalEmployeeById(employeeId);

        if (employee != null)
        {
            // calculate fields
            employee.SuggestedBonus = CalculateSuggestedBonus(employee);
        }
        return employee;
    }

    public async Task<InternalEmployee?> GetInternalEmployeeByIdAsync(Guid employeeId)
    {
        var employee = await _repository.GetInternalEmployeeByIdAsync(employeeId);

        if (employee != null)
        {
            // calculate fields
            employee.SuggestedBonus = CalculateSuggestedBonus(employee);
        }
        return employee;
    }

    public async Task<IEnumerable<InternalEmployee>> GetInternalEmployeesAsync()
    {
        var employees = await _repository.GetInternalEmployeesAsync();

        foreach (var employee in employees)
        {
            // calculate fields
            employee.SuggestedBonus = CalculateSuggestedBonus(employee);
        }

        return employees;
    }

    public async Task GiveMinimumRaiseAsync(InternalEmployee employee)
    {
        employee.Salary += 100;
        employee.MinimumRaiseGiven = true;

        // save this
        await _repository.SaveChangesAsync();
    }

    public async Task GiveRaiseAsync(InternalEmployee employee, int raise)
    {
        // raise must be at least 100
        if (raise < 100)
        {
            throw new EmployeeInvalidRaiseException(
                "Invalid raise: raise must be higher than or equal to 100.", raise);
            //throw new Exception(
            //  "Invalid raise: raise must be higher than or equal to 100."); 
        }

        // if minimum raise was previously given, the raise must 
        // be higher than the minimum raise
        if (employee.MinimumRaiseGiven && raise == 100)
        {
            throw new EmployeeInvalidRaiseException(
                "Invalid raise: minimum raise cannot be given twice.", raise);
        }

        if (raise == 100)
        {
            await GiveMinimumRaiseAsync(employee);
        }
        else
        {
            employee.Salary += raise;
            employee.MinimumRaiseGiven = false;
            await _repository.SaveChangesAsync();
        }
    }

    protected virtual void OnEmployeeIsAbsent(EmployeeIsAbsentEventArgs eventArgs)
    {
        EmployeeIsAbsent?.Invoke(this, eventArgs);
    }    

    public void NotifyOfAbsence(Employee employee)
    {
        // Employee is absent.  Other parts of the application may 
        // respond to this. Trigger the EmployeeIsAbsent event 
        // (via a virtual method so it can be overridden in subclasses)
        OnEmployeeIsAbsent(new EmployeeIsAbsentEventArgs(employee.Id));
    }
}