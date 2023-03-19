namespace Hr.Api.Business;

public class EmployeeIsAbsentEventArgs : EventArgs
{
    public Guid EmployeeId { get; private set; }

    public EmployeeIsAbsentEventArgs(Guid employeeId)
    {
        EmployeeId = employeeId;
    }
}