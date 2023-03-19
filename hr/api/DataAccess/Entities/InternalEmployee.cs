using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hr.Api.DataAccess.Entities;

public class InternalEmployee : Employee
{
    [Required]
    public int YearsOfService { get; set; }
    [Required]
    public decimal Salary { get; set; }
    [NotMapped]
    public decimal SuggestedBonus { get; set; }
    [Required]
    public bool MinimumRaiseGiven { get; set; }
    [Required]
    public int JobLevel { get; set; }
    public List<Course> AttendedCourses { get; set; } = new();

    public InternalEmployee(string firstName, string lastName, 
        int yearsOfService, decimal salary, bool minimumRaiseGiven, int jobLevel) 
        : base(firstName, lastName)
    {
        YearsOfService = yearsOfService;
        Salary = salary;
        MinimumRaiseGiven = minimumRaiseGiven;
        JobLevel = jobLevel;
    }
}
