using Serilog;
using Serilog.Events;

namespace Hr.Api.Configurations.Extensions;

public static class WebApplicationBuilderAddLoggingExtension
{
    private static void ConfigureSerilog()
    {
        var assemblyName = typeof(Program).Assembly.GetName().Name;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Assembly", assemblyName!)
            .WriteTo.Console()
            .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day)
            // available sinks: https://github.com/serilog/serilog/wiki/Provided-Sinks
            // Seq: https://datalust.co/seq
            // Seq with Docker: https://docs.datalust.co/docs/getting-started-with-docker
            .WriteTo.Seq(serverUrl: "http://seq:5341")
            .CreateLogger();
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        ConfigureSerilog();

        builder.Host.UseSerilog();

        return builder;
    }
}