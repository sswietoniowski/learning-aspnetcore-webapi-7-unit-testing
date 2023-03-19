using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hr.Api.DataAccess.Entities;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public bool IsNew { get; set; } = true;
    public string Title { get; set; } = string.Empty;
    public List<InternalEmployee> EmployeesThatAttended { get; set; } = new();

    public Course(string title)
    {
        Title = title;
    }
}