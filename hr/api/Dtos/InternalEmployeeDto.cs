using System.ComponentModel.DataAnnotations;

namespace Hr.Api.Dtos;

public class InternalEmployeeDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public int YearsOfService { get; set; }
    public decimal Salary { get; set; }
    public decimal SuggestedBonus { get; set; }
    public bool MinimumRaiseGiven { get; set; }
    public int JobLevel { get; set; }
}

public class InternalEmployeeForCreationDto
{
    [Required]
    [MaxLength(64)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(128)]
    public string LastName { get; set; } = string.Empty;
} 