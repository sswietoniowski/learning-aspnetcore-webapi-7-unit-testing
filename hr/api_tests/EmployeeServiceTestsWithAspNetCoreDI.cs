using Hr.Api.Tests.Fixtures;

namespace Hr.Api.Tests;

public class EmployeeServiceTestsWithAspNetCoreDI 
    : IClassFixture<EmployeeServiceWithAspNetCoreDIFixture>
{
    private readonly EmployeeServiceWithAspNetCoreDIFixture
        _employeeServiceFixture;

    public EmployeeServiceTestsWithAspNetCoreDI(
        EmployeeServiceWithAspNetCoreDIFixture employeeServiceFixture)
    {
        _employeeServiceFixture = employeeServiceFixture;
    }

    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse_WithObject()
    {
        // Arrange

        var obligatoryCourse = _employeeServiceFixture
            .HrTestDataRepository
            .GetCourseById(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

        // Act
        var internalEmployee = _employeeServiceFixture
            .EmployeeService.CreateInternalEmployee("Brooklyn", "Cannon");

        // Assert
        Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
    }
}