using System.ComponentModel.DataAnnotations;

namespace Hr.Api.Dtos;

public class PromotionDto
{
    public Guid EmployeeId { get; set; }
    public int JobLevel { get; set; }
}

public class PromotionForCreationDto
{
    [Required]
    public Guid EmployeeId { get; set; }
}