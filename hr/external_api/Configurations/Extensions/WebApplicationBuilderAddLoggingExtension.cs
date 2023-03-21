using Serilog;

namespace Management.Api.Configurations.Extensions;

public static class WebApplicationBuilderAddLoggingExtension
{
    private static void ConfigureSerilog(WebApplicationBuilder builder)
    {
        var assemblyName = typeof(Program).Assembly.GetName().Name;

        var configuration = builder.Configuration;

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithProperty("Assembly", assemblyName!)
            .CreateLogger();
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        ConfigureSerilog(builder);

        builder.Host.UseSerilog();

        return builder;
    }
}