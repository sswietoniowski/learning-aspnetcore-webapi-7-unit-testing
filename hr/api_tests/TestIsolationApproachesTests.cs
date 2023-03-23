using Hr.Api.Business.Services;
using Hr.Api.DataAccess;
using Hr.Api.DataAccess.Entities;
using Hr.Api.DataAccess.Repositories;
using Hr.Api.Tests.HttpMessageHandlers;
using Hr.Api.Tests.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace Hr.Api.Tests;

public class TestIsolationApproachesTests
{
    [Fact]
    public async Task AttendCourseAsync_CourseAttended_SuggestedBonusMustCorrectlyBeRecalculated()
    {
        // Arrange
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var optionsBuilder = new DbContextOptionsBuilder<HrDbContext>()
            .UseSqlite(connection);

        var dbContext = new HrDbContext(optionsBuilder.Options);
        dbContext.Database.Migrate();

        var employeeManagementDataRepository =
            new HrRepository(dbContext);

        var employeeService = new EmployeeService(
            employeeManagementDataRepository,
            new EmployeeFactory());

        // get course from database - "Dealing with Customers 101"
        var courseToAttend = await employeeManagementDataRepository
            .GetCourseByIdAsync(Guid.Parse("844e14ce-c055-49e9-9610-855669c9859b"));

        // get existing employee - "Megan Jones"
        var internalEmployee = await employeeManagementDataRepository
            .GetInternalEmployeeByIdAsync(Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"));

        if (courseToAttend == null || internalEmployee == null)
        {
            throw new XunitException("Arranging the test failed");
        }

        // expected suggested bonus after attending the course
        var expectedSuggestedBonus = internalEmployee.YearsOfService
                                     * (internalEmployee.AttendedCourses.Count + 1) * 100;

        // Act
        await employeeService.AttendCourseAsync(internalEmployee, courseToAttend);

        // Assert
        Assert.Equal(expectedSuggestedBonus, internalEmployee.SuggestedBonus);
    }

    [Fact]
    public async Task PromoteInternalEmployeeAsync_IsEligible_JobLevelMustBeIncreased()
    {
        // Arrange
        var httpClient = new HttpClient(
            new TestablePromotionEligibilityHandler(true));
        httpClient.BaseAddress = new Uri("https://localhost:5003");
        var internalEmployee = new InternalEmployee(
            "Brooklyn", "Cannon", 5, 3000, false, 1);
        var promotionService = new PromotionService(new HrTestDataRepository(), httpClient);

        // Act
        await promotionService.PromoteInternalEmployeeAsync(internalEmployee);

        // Assert
        Assert.Equal(2, internalEmployee.JobLevel);
    }

}