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