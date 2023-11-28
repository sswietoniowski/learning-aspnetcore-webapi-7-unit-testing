using BenchmarkDotNet.Running;

using Hr.Api.Benchmark;

var summary = BenchmarkRunner.Run<InternalEmployeesControllerBenchmark>();
