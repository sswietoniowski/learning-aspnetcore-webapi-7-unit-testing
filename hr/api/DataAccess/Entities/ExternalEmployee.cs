namespace Hr.Api.DataAccess.Entities;

public class ExternalEmployee : Employee
{
    public string Company { get; set; } = string.Empty;

    public ExternalEmployee(string firstName, string lastName, string company) 
        : base(firstName, lastName)
    {
        Company = company;
    }
}