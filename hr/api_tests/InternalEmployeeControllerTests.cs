using AutoMapper;
using Hr.Api.Business.Services;
using Hr.Api.Configurations.Mapper;
using Hr.Api.Controllers;
using Hr.Api.DataAccess.Entities;
using Hr.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hr.Api.Tests;

public class InternalEmployeeControllerTests
{
    private readonly InternalEmployeesController _internalEmployeesController;
    private readonly InternalEmployee _firstEmployee;

    public InternalEmployeeControllerTests()
    {
        _firstEmployee = new InternalEmployee(
            "Megan", "Jones", 2, 3000, false, 2)
        {
            Id = Guid.Parse("bfdd0acd-d314-48d5-a7ad-0e94dfdd9155"),
            SuggestedBonus = 400
        };

        var employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock
            .Setup(m => m.GetInternalEmployeesAsync())
            .ReturnsAsync(new List<InternalEmployee>() {
                _firstEmployee,
                new InternalEmployee("Jaimy", "Johnson", 3, 3400, true, 1),
                new InternalEmployee("Anne", "Adams", 3, 4000, false, 3)
            });

        //var mapperMock = new Mock<IMapper>();
        //mapperMock.Setup(m =>
        //     m.Map<InternalEmployee, Models.InternalEmployeeDto>
        //     (It.IsAny<InternalEmployee>()))
        //     .Returns(new Models.InternalEmployeeDto());
        var mapperConfiguration = new MapperConfiguration(
            cfg => cfg.AddProfile<EmployeeProfile>());
        var mapper = new Mapper(mapperConfiguration);

        _internalEmployeesController = new InternalEmployeesController(
            employeeServiceMock.Object, mapper, default!);
    }

    [Fact]
    public async Task GetInternalEmployees_GetAction_MustReturnOkObjectResult()
    {
        // Arrange

        // Act
        var result = await _internalEmployeesController.GetInternalEmployees();

        // Assert
        var actionResult = Assert
            .IsType<ActionResult<IEnumerable<InternalEmployeeDto>>>(result);
        Assert.IsType<OkObjectResult>(actionResult.Result);

    }


    [Fact]
    public async Task GetInternalEmployees_GetAction_MustReturnIEnumerableOfInternalEmployeeDtoAsModelType()
    {
        // Arrange

        // Act 
        var result = await _internalEmployeesController.GetInternalEmployees();

        // Assert
        var actionResult = Assert
            .IsType<ActionResult<IEnumerable<InternalEmployeeDto>>>(result);

        Assert.IsAssignableFrom<IEnumerable<InternalEmployeeDto>>(
            ((OkObjectResult)actionResult!.Result!).Value);
    }

    [Fact]
    public async Task GetInternalEmployees_GetAction_MustReturnNumberOfInputtedInternalEmployees()
    {
        // Arrange

        // Act
        var result = await _internalEmployeesController.GetInternalEmployees();

        // Assert
        var actionResult = Assert
            .IsType<ActionResult<IEnumerable<InternalEmployeeDto>>>(result);

        Assert.Equal(3,
            ((IEnumerable<InternalEmployeeDto>)
                ((OkObjectResult)actionResult!.Result!).Value!).Count());
    }

    [Fact]
    public async Task GetInternalEmployees_GetAction_ReturnsOkObjectResultWithCorrectAmountOfInternalEmployees()
    {
        // Arrange

        // Act
        var result = await _internalEmployeesController.GetInternalEmployees();

        // Assert
        var actionResult = Assert
            .IsType<ActionResult<IEnumerable<InternalEmployeeDto>>>(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var dtos = Assert.IsAssignableFrom<IEnumerable<InternalEmployeeDto>>
            (okObjectResult.Value);
        Assert.Equal(3, dtos.Count());
        var firstEmployee = dtos.First();
        Assert.Equal(_firstEmployee.Id, firstEmployee.Id);
        Assert.Equal(_firstEmployee.FirstName, firstEmployee.FirstName);
        Assert.Equal(_firstEmployee.LastName, firstEmployee.LastName);
        Assert.Equal(_firstEmployee.Salary, firstEmployee.Salary);
        Assert.Equal(_firstEmployee.SuggestedBonus, firstEmployee.SuggestedBonus);
        Assert.Equal(_firstEmployee.YearsOfService, firstEmployee.YearsOfService);
    }
}