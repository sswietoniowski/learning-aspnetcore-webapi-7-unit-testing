namespace Hr.Api.Business.Events;

public class EmployeeIsAbsentEventArgs : EventArgs
{
    public Guid EmployeeId { get; private set; }

    public EmployeeIsAbsentEventArgs(Guid employeeId)
    {
        EmployeeId = employeeId;
    }
}