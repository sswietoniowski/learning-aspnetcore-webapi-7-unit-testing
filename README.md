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

## Unit Testing ASP.NET Core API Controllers

## Unit Testing ASP.NET Core Middleware, Filters and Service Registrations

## Integrating Unit Tests In Your Development and Release Flows

TODO: FluentAssertions
