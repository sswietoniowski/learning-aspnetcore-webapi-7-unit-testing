# Learning ASP.NET Core - WebAPI (.NET 7) Unit Testing

This repository contains examples showing how to unit test an API (WebAPI 7).

Based on this course [Unit Testing an ASP.NET Core 6 Web API](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-web-api-unit-testing/table-of-contents).

Original course materials can be found [here](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-web-api-unit-testing/exercise-files) and [here](https://github.com/KevinDockx/UnitTestingAspNetCore6WebAPI).

## Setup

TODO:

## Introduction

> _Unit test_ is an automated test that tests a small piece of behavior.

The What, Why and What Not of Unit Testing:

- unit tests should have low complexity,
- unit tests should be fast,
- unit tests should be well encapsulated,
- a unit test does not test the whole system,
- a unit test does not test how parts of a system that are related to each other interact.

Reasons for Test Automation:

- improved reliability at a relatively low cost,
- write once, use without additional cost,
- enables testing often and multiple times,
- bugs are found faster and easier (makes them cheaper to fix).

Most applications should be tested with a combination of automated tests:

- unit tests,
- integration tests,
- functional (end-to-end) tests.

> _Integration test_ is an automated test that tests whether or not two or more components work together correctly.

Integration test can test a full request/response cycle, but doesn't have to. Can be created with the same framework as unit tests - optionally combined with Microsoft TestHost and TestServer.

Integration tests characteristics:

- integration tests have medium complexity,
- integration tests are relatively slow,
- integration tests are not well encapsulated.

> _Functional test_ is an automated test that tests the full request/response cycle of an application.

Functional tests can be automated with:

- Selenium (web applications),
- Postman (APIs),
- Microsoft TestHost and TestServer.

Functional tests characteristics:

- functional tests have high complexity,
- functional tests are slow,
- functional tests are badly encapsulated.

Good and Bad Candidates for a Unit Test:

- good candidates: algorithms, behavior, rules,
- bad candidates: data access, UI, system interactions.

Naming Guidelines for Unit Tests:

`CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500`

`CreateEmployee` - a name for the unit that's being tested.

`ConstructInternalEmployee` - the scenario under which the unit is being tested.

`SalaryMustBe2500` - the expected behavior when the scenario is invoked.

The Arrange, Act, Assert Pattern:

- _Arrange_ - setting up the test,
- _Act_ - executing the actual test,
- _Assert_ - verifying the executed action.

Example:

```csharp
[Fact]
public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500()
{
    // Arrange
    var employeeFactory = new EmployeeFactory();
    // Act
    var employee = (InternalEmployee)employeeFactory.CreateEmployee("John", "Doe");
    // Assert
    Assert.Equal(2500, employee.Salary);
}
```

Comparing xUnit, NUnit and MSTest:

- MSTest - Microsoft's built-in unit test framework, support for .NET (Core) since v2.0,
- NUnit - open source, a port of JUnit, been around for a long time,
- xUnit - built with .NET (Core) and new .NET features in mind.

MSTest and NUnit can be used to test .NET 6 code, but they carry technical debt with them. Designed
nor coded with .NET Core or .NET 6 in mind.

xUnit is successor of NUnit, built with .NET (Core) and new .NET features in mind. Improves test isolation,
and extensibility. Encourages cleaner testing code.

## Basic Scenarios

TODO:

## Setting Up Tests and Controlling Test Execution

TODO:

## Working with Data-driven Tests

TODO:

## Isolating Unit Tests with ASP.NET Core Techniques and Mocking

## Unit Testing ASP.NET Core API Controllers

## Unit Testing ASP.NET Core Middleware, Filters and Service Registrations

## Integrating Unit Tests In Your Development and Release Flows
