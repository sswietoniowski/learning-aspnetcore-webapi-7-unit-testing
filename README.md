[![Run tests](https://github.com/sswietoniowski/learning-aspnetcore-webapi-7-unit-testing/actions/workflows/run-tests.yaml/badge.svg?branch=master)](https://github.com/sswietoniowski/learning-aspnetcore-webapi-7-unit-testing/actions/workflows/run-tests.yaml)

# Learning ASP.NET Core - WebAPI (.NET 7) Unit Testing

This repository contains examples showing how to unit test an API (WebAPI 7).

Based on this course [Unit Testing an ASP.NET Core 6 Web API](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-web-api-unit-testing/table-of-contents).

Original course materials can be found [here](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-web-api-unit-testing/exercise-files) and [here](https://github.com/KevinDockx/UnitTestingAspNetCore6WebAPI).

## Table of Contents

- [Learning ASP.NET Core - WebAPI (.NET 7) Unit Testing](#learning-aspnet-core---webapi-net-7-unit-testing)
  - [Table of Contents](#table-of-contents)
  - [Setup](#setup)
  - [Introduction to Unit Testing](#introduction-to-unit-testing)
  - [Tackling Basic Unit Testing Scenarios](#tackling-basic-unit-testing-scenarios)
  - [Setting Up Tests and Controlling Test Execution](#setting-up-tests-and-controlling-test-execution)
  - [Working with Data-driven Tests](#working-with-data-driven-tests)
  - [Isolating Unit Tests with ASP.NET Core Techniques and Mocking](#isolating-unit-tests-with-aspnet-core-techniques-and-mocking)
  - [Unit Testing ASP.NET Core API Controllers](#unit-testing-aspnet-core-api-controllers)
  - [Unit Testing ASP.NET Core Middleware, Filters and Service Registrations](#unit-testing-aspnet-core-middleware-filters-and-service-registrations)
  - [Integrating Unit Tests In Your Development and Release Flows](#integrating-unit-tests-in-your-development-and-release-flows)
  - [Extras](#extras)
    - [Evaluating and Benchmarking the Performance of APIs with k6](#evaluating-and-benchmarking-the-performance-of-apis-with-k6)
    - [Benchmarking APIs with BenchmarkDotNet](#benchmarking-apis-with-benchmarkdotnet)
    - [How to comprehensively (E2E) test a Web API?](#how-to-comprehensively-e2e-test-a-web-api)
  - [Summary](#summary)

## Setup

First run external (Management) API:

```cmd
cd .\hr\external_api
dotnet restore
dotnet build
dotnet watch run
cd ..
```

Then run main (HR) API:

```cmd
cd .\hr\api
dotnet restore
dotnet build
dotnet watch run
cd ..
```

Or you can use Docker Compose and run the following command:

```cmd
docker-compose --file .\hr\docker-compose.yml --project-name hr up --build -d
```

To run tests:

```cmd
cd .\hr\api_tests
dotnet restore
dotnet build
dotnet test
```

```cmd
cd .\hr\library_tests
dotnet restore
dotnet build
dotnet test
```

All tests should pass.

Alternatively, you can use Visual Studio and Test Explorer.

## Introduction to Unit Testing

> _Unit test_ is an automated test that tests a small piece of behavior.

The What, Why, and What Not of Unit Testing:

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

Integration tests can test a full request/response cycle but don't have to. Can be created with the same framework as unit tests - optionally combined with Microsoft TestHost and TestServer.

Integration test characteristics:

- integration tests have medium complexity,
- integration tests are relatively slow,
- integration tests are not well encapsulated.

> _Functional test_ is a (not necessarily automated) test that tests the full request/response cycle of an application.

Functional tests can be automated with:

- Selenium (web applications),
- Postman (APIs),
- Microsoft TestHost and TestServer.

Functional test characteristics:

- functional tests have high complexity,
- functional tests are slow,
- functional tests are badly encapsulated.

Good and Bad Candidates for a Unit Test:

- good candidates: algorithms, behavior, rules,
- bad candidates: data access, UI, system interactions.

Usually, you'll have a mix of unit tests, integration tests, and functional tests. The majority of tests should be unit tests, then integration tests, and finally (minority) functional tests.

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

xUnit is the successor of NUnit, built with .NET (Core) and new .NET features in mind. Improves test isolation,
and extensibility. Encourages cleaner testing code.

## Tackling Basic Unit Testing Scenarios

> _Assert_ is a boolean expression used to verify the outcome of a test that should be evaluated as true.

A test can contain one or more asserts:

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

Asserts on async tasks:

```csharp
Assert.Equal(1, await Task.Run(() => 1));
Assert.NotEqual(1, await Task.Run(() => 2));
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

_Constructor and dispose approach_: Set up the test context in the constructor and potentially clean it up in the Dispose method (the context is recreated for each test).

_Class fixture approach_ - create a single test context shared among all tests in the class. Context is cleaned up after all tests in the class have been executed. Use when context creation and cleanup are expensive.

_Class fixture_ - don't let a test depend on changes made to the context by other tests. Tests must remain isolated. You don't have control over the order in which tests are executed.

_Collection fixture approach_ - create a single test context shared among tests in several test classes. Context is cleaned up after all tests across classes have been executed. Use when context creation and cleanup are expensive.

Integrating test context with ASP.NET Core dependency injection container.

In ASP.NET Core, dependencies are often resolved via the built-in IoC container. Could you integrate that with a unit test? Newing up dependencies is the preferred approach - simple, fast, and concise. You might want to integrate
with the DI system. If the class has many dependencies and if the dependency tree is large.

## Working with Data-driven Tests

Introducing Theories and Data-driven Tests

> _Fact_ a test that is always true. They test invariant conditions.

Instead of many tests with the same code but different input data, you can use a single test with multiple input data sets. To do so we are replacing the `[Fact]` attribute with `[Theory]` attribute. `[Theory]` attribute marks a test method as a data-driven test. `[Theory]` attribute can be used with `[InlineData]` attribute to provide input data for the test. `[InlineData]` attribute provides input data for the test. `[InlineData]` attribute can be used multiple times to provide multiple input data sets for the test.

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

By isolating a test, you can be sure that it's the cause of the code under test when it passes or fails.

> _Test double_ is a generic term for any case where you replace a production object for testing purposes.

Test doubles:

- _Fake_ a working implementation not suitable for production use,
- _Dummy_ a test double that's never accessed or used,
- _Stub_ a test double that provides fake data to the system under test,
- _Spy_ a test double capable of capturing indirect output and providing indirect input as needed,
- _Mock_ a test double that implements the expected behavior.

Test isolation approaches:

- manually creating test doubles,
- using a built-in framework or library functionality to create test doubles,
- using a mocking framework to create test doubles.

Different types of test doubles and different approaches are often combined. Focus on the fact that the test is isolated, no matter how.

Unit testing with Entity Framework Core.

EF Core contains a set of built-in functionalities to enable testing & test isolation easily:

- avoid calling into a real database,
- use in-memory implementations instead.

_In-memory database provider_ - simple scenarios are not the best option.

_SQLite in-memory mode_ - best compatibility with real databases.

Unit testing with HttpClient.

Tests must be isolated from network calls. A custom message handler can short-circuit the actual call.

Which test isolation approach should you use?

Consider:

- test reliability,
- the effort required to create test doubles,
- available knowledge and experience.

## Unit Testing ASP.NET Core API Controllers

> Test the behavior you coded yourself. Don't test the framework.

Code coverage and deciding what to unit test.

Steer away from generalizations like "test everything", "don't test repositories", "test only the public API", etc.:

- architectures, pattern implementations, ... often differ from project to project,
- so-called best practices are sometimes diverted from, on purpose or accidentally.

Trying to achieve 100% code coverage can be counterproductive. It's not about the number of tests. It is about the quality of tests.
ROI from writing the last 10% might not be worth it.

> A high code coverage percentage does not indicate success or code quality. A high code coverage percentage only truly represents the code covered by tests.

Controller Types:

- _thick controllers_ - contain logic that implements the expected behavior; this is code that should be unit tested,
- _thin or skinny controllers_ - delegate the actual implementation of the behavior to other components; these typically don't need to be unit tested.

Introduction to testing MVC controllers.

A variety of reasons can lead to choosing thin or thick controllers; one isn't, by definition, better than the other.

This can lead to a different decision in regard to whether controllers should be unit-tested or not.

You don't always have the luxury to decide:

- you may get thrown into an existing project halfway through,
- you may need to improve the reliability of an existing, finished application by writing tests.

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

> `HttpContext` is an object that encapsulates all HTTP-specific information about an individual HTTP request: a container for a single request.

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

Dependencies that are difficult to mock can lead to an integration test. Mostly
though a unit test is advisable for middleware testing.

Typical concerns when unit testing middleware:

- mock the `HttpContext` (or use `DefaultHttpContext`),
- handle the `RequestDelegate`.

> _ASP.NET Core filter_ - a filter that allows code to run before or after specific stages in the request processing pipeline.

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
These registrations can be unit-tested.

Approach:

- create an `IServiceCollection` instance,
- register the services you want to test,
- build an `IServiceProvider` instance,
- verify whether the services are registered correctly.

## Integrating Unit Tests In Your Development and Release Flows

Running tests with the CLI.

> _Test runner_ - the program (or maybe a third-party plugin to a program) that is responsible for looking for one or more assemblies with
> tests in them and activating the test frameworks that it finds in those assemblies.
>
> _Test framework_ - the code that has detailed knowledge of how to discover and run unit tests.

Running tests in parallel allows a set of tests to finish faster, locally and on your build server.

Test runner - a runner can support running different test assemblies in parallel.

Test framework - a framework that can support running tests within a single assembly in parallel.

Running tests against multiple target frameworks.

xUnit supports running tests against multiple target frameworks, useful when developing frameworks or libraries. You want to ensure your library correctly functions across target frameworks.

Integrating unit tests in your CI/CD pipeline.

![Integrating unit tests in your CI/CD pipeline](./img/unit_tests_in_cicd_pipeline.jpg)

## Extras

Couple of things that I found interesting.

### Evaluating and Benchmarking the Performance of APIs with k6

Based on [this](https://learning.oreilly.com/library/view/mastering-minimal-apis/9781803237824/B17902_10.xhtml) book.

To test load on a web application and determine how many requests per second it can handle, we can use the [**k6**](https://github.com/grafana/k6) tool.

Use cases for the k6 tool:

- load testing,
- performance and synthetic monitoring,
- chaos and reliability testing.

To install the k6 tool, run the following command (provided that you are using [Chocolatey](https://chocolatey.org/)):

```cmd
choco install k6
```

If you've got it already installed, you can update it with the following command:

```cmd
choco upgrade k6
```

Let's assume that we want to test the following endpoint:

```http
GET https://localhost:5001/api/internal-employees
```

To do that we need to create a JavaScript file with the following content:

```javascript
import http from 'k6/http';
import { check } from 'k6';
import { Rate } from 'k6/metrics';

const checkFailureRate = new Rate('check_failure_rate');

export let options = {
  summaryTrendStats: ['avg', 'p(95)'],
  stages: [
    // Linearly ramp up from 1 to 50 VUs during 10 seconds
    { target: 50, duration: '10s' },

    // Hold at 50 VUs for the next 1 minute
    { target: 50, duration: '1m' },

    // Linearly ramp down from 50 to 0 VUs over the last 15 seconds
    { target: 0, duration: '15s' },
  ],

  thresholds: {
    // We want the 95th percentile of all HTTP request durations to be less than 500ms
    http_req_duration: ['p(95)<500'],

    // Thresholds based on the custom metric we defined and use to track application failures
    check_failure_rate: [
      // Global failure rate should be less than 1%
      'rate<0.01',

      // Abort the test early if it climbs over 5%
      { threshold: 'rate<=0.05', abortOnFail: true },
    ],
  },
};

export default function () {
  // execute http get call
  let response = http.get('https://localhost:5001/api/internal-employees');

  // check() returns false if any of the specified conditions fail
  const result = check(response, {
    'status is 200': (r) => r.status === 200,
  });

  // We reverse the check() result since we want to count the failures
  checkFailureRate.add(!result);
}
```

Now we can start our API (I'm using Docker Compose):

```cmd
docker-compose --file .\hr\docker-compose.yml --project-name hr up --build -d
```

Then we can run our test:

```cmd
k6 run .\hr\api_tests\k6\load_test.js --summary-export=.\hr\api_tests\k6\results\load_test_results.json
```

After the test is finished, we can see the results (in your case they might be different):

```cmd

          /\      |‾‾| /‾‾/   /‾‾/
     /\  /  \     |  |/  /   /  /
    /  \/    \    |     (   /   ‾‾\
   /          \   |  |\  \ |  (‾)  |
  / __________ \  |__| \__\ \_____/ .io

  execution: local
     script: .\hr\api_tests\k6\load_test.js
     output: -

  scenarios: (100.00%) 1 scenario, 50 max VUs, 1m55s max duration (incl. graceful stop):
           * default: Up to 50 looping VUs for 1m25s over 3 stages (gracefulRampDown: 30s, gracefulStop: 30s)


     ✓ status is 200

   ✓ check_failure_rate.............: 0.00%   ✓ 0           ✗ 104112
     checks.........................: 100.00% ✓ 104112      ✗ 0
     data_received..................: 48 MB   570 kB/s
     data_sent......................: 4.1 MB  49 kB/s
     http_req_blocked...............: avg=7.77µs  p(95)=0s
     http_req_connecting............: avg=288ns   p(95)=0s
   ✓ http_req_duration..............: avg=34.77ms p(95)=64.12ms
       { expected_response:true }...: avg=34.77ms p(95)=64.12ms
     http_req_failed................: 0.00%   ✓ 0           ✗ 104112
     http_req_receiving.............: avg=1.12ms  p(95)=7.97ms
     http_req_sending...............: avg=63.59µs p(95)=529.5µs
     http_req_tls_handshaking.......: avg=6.93µs  p(95)=0s
     http_req_waiting...............: avg=33.58ms p(95)=60.99ms
     http_reqs......................: 104112  1224.799898/s
     iteration_duration.............: avg=34.88ms p(95)=64.28ms
     iterations.....................: 104112  1224.799898/s
     vus............................: 1       min=1         max=50
     vus_max........................: 50      min=50        max=50


running (1m25.0s), 00/50 VUs, 104112 complete and 0 interrupted iterations
default ✓ [======================================] 00/50 VUs  1m25s
```

### Benchmarking APIs with BenchmarkDotNet

[BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) is a framework for benchmarking .NET code.

This tool is used for calculating the time taken for the execution of a task, the memory used, and many other parameters.

To use this tool, I've created a new project:

```cmd
dotnet new console --name Hr.Api.Benchmark --output .\hr\api_benchmark
```

And added this project to the solution:

```cmd
dotnet sln .\hr\Hr.sln add .\hr\api_benchmark\Hr.Api.Benchmark.csproj
```

Then I added the following dependencies:

```cmd
dotnet add .\hr\api_benchmark\Hr.Api.Benchmark.csproj package BenchmarkDotNet
```

In the `Program.cs` file I've added the following code:

```csharp
using BenchmarkDotNet.Running;

using Hr.Api.Benchmark;

var summary = BenchmarkRunner.Run<InternalEmployeesControllerBenchmark>();
```

Then I've created a new class `InternalEmployeesControllerBenchmark.cs` in the `Hr.Api.Benchmark` project:

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Hr.Api.Benchmark;

[SimpleJob(RuntimeMoniker.Net70, baseline: true)]
[JsonExporter]
public class InternalEmployeesControllerBenchmark
{
    private readonly HttpClient client = new HttpClient();

    [GlobalSetup]
    public void Setup()
    {
        client.BaseAddress = new Uri("https://localhost:5001");
    }

    [Benchmark]
    public async Task InternalEmployees_Get() => await client.GetAsync("api/internal-employees");
}
```

Now we can run our benchmark.

First, we need to start our API (I'm using Docker Compose):

```cmd
docker-compose --file .\hr\docker-compose.yml --project-name hr up --build -d
```

Then run the following command:

```cmd
cd .\hr\api_benchmark
dotnet run --configuration Release
```

Finally, we can see the results:

```cmd
BenchmarkDotNet v0.13.10, Windows 10 (10.0.19045.3693/22H2/2022Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.311
  [Host]   : .NET 7.0.14 (7.0.1423.51910), X64 RyuJIT AVX2
  .NET 7.0 : .NET 7.0.14 (7.0.1423.51910), X64 RyuJIT AVX2

Job=.NET 7.0  Runtime=.NET 7.0

| Method                | Mean     | Error     | StdDev    | Ratio |
|---------------------- |---------:|----------:|----------:|------:|
| InternalEmployees_Get | 1.922 ms | 0.0355 ms | 0.0510 ms |  1.00 |

// * Hints *
Outliers
  InternalEmployeesControllerBenchmark.InternalEmployees_Get: .NET 7.0 -> 2 outliers were removed (2.06 ms, 2.12 ms)

// * Legends *
  Mean   : Arithmetic mean of all measurements
  Error  : Half of 99.9% confidence interval
  StdDev : Standard deviation of all measurements
  Ratio  : Mean of the ratio distribution ([Current]/[Baseline])
  1 ms   : 1 Millisecond (0.001 sec)

// ***** BenchmarkRunner: End *****
Run time: 00:00:21 (21.56 sec), executed benchmarks: 1

Global total time: 00:00:25 (25.5 sec), executed benchmarks: 1
// * Artifacts cleanup *
Artifacts cleanup is finished
```

### How to comprehensively (E2E) test a Web API?

[Here](https://youtu.be/_d8umg11YQw?si=qJbfBSpnjRUei2-o) you will find a great explanation of how to test a Web API (End-to-End).

Also, please watch [this](https://youtu.be/m7r2qyUabTs?si=hTyBQvh2sCehxSXg) video.

## Summary

Are you ready to start testing :-)?
