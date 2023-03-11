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

> _Functional test_ is a (not necessarily automated) test that tests the full request/response cycle of an application.

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

Usually you'll have a mix of unit tests, integration tests and functional tests. Majority of tests should be unit tests, then integration tests and finally (minority) functional tests.

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

> _Assert_ is a boolean expression, used to verify the outcome of a test, that should evaluate to true.

A test can contain on or more asserts:

- fails when **one or more** asserts fail,
- passes when **all** asserts pass.

xUnit provides asserts for all common core testing scenarios.

> A unit test should only contain one assert ...

Quote by: "The strict school of thought" ;-)

A unit is a small piece of behavior that you want to test. Multiple assertions in one test are acceptable if they
assert the same behavior.

> It's not about the amount of asserts you're using in a test, it's about the behavior you're testing.

Different assertions examples.

Asserts on booleans:

```csharp
Assert.True(true);
Assert.False(false);
```

Asserts on strings:

```csharp
Assert.Equal("Hello", "Hello");
Assert.NotEqual("Hello", "World");
Assert.Contains("Hello", "Hello World");
Assert.DoesNotContain("Hello", "World");
Assert.StartsWith("Hello", "Hello World");
Assert.EndsWith("World", "Hello World");
Assert.Matches("Hello", "Hello World");
Assert.DoesNotMatch("Hello", "World");
```

Asserts on numeric values:

```csharp
Assert.Equal(1, 1);
Assert.NotEqual(1, 2);
Assert.InRange(1, 0, 2);
Assert.NotInRange(1, 2, 3);
Assert.Greater(2, 1);
Assert.GreaterOrEqual(2, 1);
Assert.Less(1, 2);
Assert.LessOrEqual(1, 2);
```

In case of floating point numbers, use `Assert.Equal` with a precision value:

```csharp
Assert.Equal(1.0, 1.0000000000000001, 5); // 5 is the precision, that is we are comparing up to 5 decimal places
```

Asserts on collections:

```csharp
Assert.Empty(new List<int>());
Assert.NotEmpty(new List<int> { 1 });
Assert.Contains(1, new List<int> { 1 });
Assert.DoesNotContain(1, new List<int> { 2 });
Assert.Single(new List<int> { 1 });
Assert.NotSingle(new List<int> { 1, 2 });
Assert.Equal(new List<int> { 1, 2 }, new List<int> { 1, 2 });
Assert.NotEqual(new List<int> { 1, 2 }, new List<int> { 1, 3 });
Assert.All(new List<int> { 1, 2 }, x => x > 0);
Assert.NotAll(new List<int> { 1, 2 }, x => x > 0);
```

Asserts on exceptions:

```csharp
Assert.Throws<ArgumentNullException>(() => throw new ArgumentNullException());
Assert.ThrowsAny<ArgumentNullException>(() => throw new ArgumentNullException());
Assert.ThrowsAsync<ArgumentNullException>(async () => await Task.Run(() => throw new ArgumentNullException()));
Assert.ThrowsAnyAsync<ArgumentNullException>(async () => await Task.Run(() => throw new ArgumentNullException()));
```

Asserts on events:

```csharp
Assert.Raises<EventArgs>(
    handler => myObject.MyEvent += handler,
    handler => myObject.MyEvent -= handler,
    () => myObject.RaiseMyEvent());
```

Asserts on objects:

```csharp
Assert.Same(myObject, myObject);
Assert.NotSame(myObject, new object());
Assert.IsType<MyObject>(myObject);
Assert.IsNotType<MyObject>(new object());
Assert.IsAssignableFrom<MyObject>(myObject);
Assert.IsNotAssignableFrom<MyObject>(new object());
```

Asserting on private methods. A private method is an implementation detail that doesn't exist in isolation. Test
the behavior of the public method that uses the private method. Making a private method public just for testing
purposes is a code smell (it violates the principle of encapsulation). As a slightly less bad alternative, you
can use `[InternalsVisibleTo]` attribute to make a private method visible to the test assembly.

## Setting Up Tests and Controlling Test Execution

_Constructor and dispose approach_ - set up test context in the constructor, potentially clean up in Dispose method (context is recreated for each test).

_Class fixture approach_ - create a single test context shared among all tests in the class. Context is cleaned up after all tests in the class have been executed. Use when context creation and cleanup is expensive.

_Class fixture_ - don't let a test depend on changes made to the context by other tests. Tests must remain isolated. You don't have control over the order in which tests are executed.

_Collection fixture approach_ - create a single test context shared among tests in several test classes. Context is cleaned up after all tests across classes have been executed. Use when context creation and cleanup is expensive.

Integrating test context with ASP.NET Core dependency injection container.

In ASP.NET Core, dependencies are often resolved via the built-in IoC container. Can that be integrated with a unit test? Newing up dependencies is the preferred approach - simple, fast, concise. You might want to integrate
with the DI system. If the class has got a lot of dependencies, if the dependency tree is large.

## Working with Data-driven Tests

Introducing Theories and Data-driven Tests

> _Fact_ a test that is always true. They test invariant conditions.

Instead of many tests with the same code, but different input data, you can use a single test with multiple input data sets. To do so we are replacing the `[Fact]` attribute with `[Theory]` attribute. `[Theory]` attribute is used to mark a test method as a data-driven test. `[Theory]` attribute can be used with `[InlineData]` attribute to provide input data for the test. `[InlineData]` attribute is used to provide input data for the test. `[InlineData]` attribute can be used multiple times to provide multiple input data sets for the test.

> _Theory_ a test which is only true for a particular set of data.

Example of a data-driven test:

```csharp
[Theory]
[InlineData(1, 1, 2)]
[InlineData(1, 2, 3)]
[InlineData(2, 2, 4)]
public void Add(int a, int b, int expected)
{
    Assert.Equal(expected, a + b);
}
```

Use `TheoryData` for type-safe data-driven tests. We can use `[InlineData]`, `[MemberData]` and `[ClassData]` attributes to provide input data for the test. `[InlineData]` attribute is used to provide input data for the test. `[MemberData]` attribute is used to provide input data for the test from a static method or property. `[ClassData]` attribute is used to provide input data for the test from a class that implements `IEnumerable<object[]>` interface.

Example of `[MemberData]` attribute:

```csharp
public static IEnumerable<object[]> Data =>
    new List<object[]>
    {
        new object[] { 1, 1, 2 },
        new object[] { 1, 2, 3 },
        new object[] { 2, 2, 4 }
    };
```

```csharp
[Theory]
[MemberData(nameof(Data))]
public void Add(int a, int b, int expected)
{
    Assert.Equal(expected, a + b);
}
```

Example of `[ClassData]` attribute:

```csharp
public class Data : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 1, 1, 2 };
        yield return new object[] { 1, 2, 3 };
        yield return new object[] { 2, 2, 4 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

```csharp
[Theory]
[ClassData(typeof(Data))]
public void Add(int a, int b, int expected)
{
    Assert.Equal(expected, a + b);
}
```

Example of `TheoryData` (strongly typed):

```csharp
public class Data : TheoryData<int, int, int>
{
    public Data()
    {
        Add(1, 1, 2);
        Add(1, 2, 3);
        Add(2, 2, 4);
    }
}
```

```csharp
[Theory]
[ClassData(typeof(Data))]
public void Add(int a, int b, int expected)
{
    Assert.Equal(expected, a + b);
}
```

Getting data from an external source. Test data can come from an external source. Other people can manage it.
Convenient for, for example, the QA team.

## Isolating Unit Tests with ASP.NET Core Techniques and Mocking

Investigating test isolation approaches.

Unit tests should be isolated from other components of the system:

- database, file system, network, etc.,
- other dependencies like factories, repositories, services, etc.

By isolating a test you can ber sure that when it passes or fails, it's the cause of the code under test.

> _Test double_ a generic term for any case where you replace a production object for testing purposes.

Test doubles:

- _Fake_ a working implementation not suitable for production use,
- _Dummy_ a test double that's never accessed or used,
- _Stub_ a test double that provides fake data to the system under test,
- _Spy_ a test double capable of capturing indirect output and providing indirect input as needed,
- _Mock_ a test double that implements the expected behavior.

Test isolation approaches:

- manually creating test doubles,
- using built-in framework or library functionality to create test doubles,
- using a mocking framework to create test doubles.

Different types of test doubles and different approaches are often combined. Focus on the fact that the test is isolated, no matter how.

Unit testing with Entity Framework Core.

EF Core contains a set of built-in functionalities to easily enable testing & test isolation:

- avoid calling into a real database,
- use in-memory implementations instead.

_In-memory database provider_ - simple scenarios, not the best option.

_SQLite in-memory mode_ - best compatibility with real databases.

Unit testing with HttpClient.

Tests mus be isolated from network calls. A custom message handler can short-circuit the actual call.

Which test isolation approach should you use?

Consider:

- test reliability,
- effort required to create test doubles,
- available knowledge and experience.

## Unit Testing ASP.NET Core API Controllers

> Test the behavior you coded yourself. Don't test the framework.

Code coverage and deciding what to unit test.

Steer away from generalizations like "test everything", "don't test repositories", "test only the public API", etc.:

- architectures, pattern implementations, ... often differ from project to project,
- so-called best practices are sometimes diverted from, on purpose or accidentally.

Trying to achieve 100% code coverage can be counterproductive. It's not about the number of tests, it's about the quality of tests.
ROI from writing the last 10% might not be worth it.

> A high code coverage percentage is not an indicator of success, or of code quality. A high code coverage percentage only truly represents the amount of code that is covered by tests.

Controller Types:

- _thick controllers_ - contain logic that implements the expected behavior; this is code that should be unit tested,
- _thin or skinny controllers_ - delegate the actual implementation of the behavior to other components; these typically don't need to be unit tested.

Introduction to testing MVC controllers.

A variety of reasons can lead to choosing for thin or thick controllers, one isn't by definition better than the other.

Can lead to a different decision in regards to whether controllers should be unit tested or not.

You don't always have the luxury to decide:

- you may get thrown into an existing project halfway through,
- you may need to improve reliability of an existing, finished application by writing tests.

Not every application is built with the same level of quality.

Automated tests can improve an application's reliability (potentially on the way to refactoring). Taking a pragmatic approach to unit testing can be valuable.

Test isolation is important, avoid model binding, filters, routing, ...

What we can test:

- expected return type,
- expected type of the returned data,
- expected values of the returned data,
- other action logic that's not framework-related code.

Concerns when unit testing controllers:

- mocking controller dependencies,
- working with `ModelState` in unit tests,
- dealing with `TempData`,
- dealing with `HttpContext.Session`,
- working with `HttpClient` calls in tests,
- ...

> `HttpContext` an object which encapsulates all HTTP-specific information about an individual HTTP request: a container for a single request.

Common information in `HttpContext`:

- request,
- response,
- features (connection, server info, ...),
- user,
- session.

Testing with `HttpContext`:

- use the built-in default implementation: `DefaultHttpContext`,
- use `Moq` for mocking: `Mock<HttpContext>`.

## Unit Testing ASP.NET Core Middleware, Filters and Service Registrations

Unit testing middleware.

Test custom middleware, not built-in middleware.

Dependencies that are difficult to mock can lead towards an integration test. Mostly
though, a unit test is advisable for middleware testing.

Typical concerns when unit testing middleware:

- mock the `HttpContext` (or use `DefaultHttpContext`),
- handle the `RequestDelegate`.

> _ASP.NET Core filter_ - a filter allows code to run before or after specific stages in the request processing pipeline.

Custom filters often handle cross-cutting concerns:

- error handling,
- caching.

Filters can be used to avoid code duplication.

Filters run in the ASP.NET Core action invocation pipeline. They can be used to:

- action filter,
- authorization filter,
- resource filter,
- exception filter,
- result filter.

Action filters:

- run immediately before and after the action method is called,
- can change the arguments passed to the action method,
- can change the result returned from the action.

Unit testing service registrations.

Services are registered on ASP.NET Core's included IoC container.
These registrations can be unit tested.

Approach:

- create an `IServiceCollection` instance,
- register the services you want to test,
- build an `IServiceProvider` instance,
- verify whether the services are registered correctly.

## Integrating Unit Tests In Your Development and Release Flows

TODO: FluentAssertions
