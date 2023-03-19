using Hr.Api.DataAccess.Entities;

namespace Hr.Api.Business.Services;

public class EmployeeFactory
{
    private readonly int _initialServiceYears = 0;
    private readonly decimal _initialSalary = 2500m;
    private readonly bool _initialRaiseGiven = false;
    private readonly int _initialJobLevel = 1;

    /// <summary>
    /// Create an employee.
    /// </summary>
    /// <param name="firstName">first name</param>
    /// <param name="lastName">last name</param>
    /// <param name="company">company</param>
    /// <param name="isExternal">flag: false -> is internal, true -> is external</param>
    /// <returns></returns>
    public virtual Employee CreateEmployee(string firstName, string lastName, string? company = null, bool isExternal = false)
    {
       if (string.IsNullOrEmpty(firstName))
       {
           throw new ArgumentException($"'{nameof(firstName)}' cannot be null or empty.", nameof(firstName));
       }

       if (string.IsNullOrEmpty(lastName))
       {
           throw new ArgumentException($"'{nameof(lastName)}' cannot be null or empty.", nameof(lastName));
       }

       if (string.IsNullOrEmpty(company) && isExternal)
       {
           throw new ArgumentException($"'{nameof(company)}' cannot be null if '{nameof(isExternal)}' is true.", nameof(company));
       }

       return isExternal
           ? new ExternalEmployee(firstName, lastName, company!)
           : new InternalEmployee(firstName, lastName, _initialServiceYears, _initialSalary, _initialRaiseGiven, _initialJobLevel);
    }
}