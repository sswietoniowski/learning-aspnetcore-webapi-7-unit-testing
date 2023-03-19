namespace Hr.Api.Configurations.Extensions;

using Books.Api.Configurations.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class WebApplicationBuilderAddGlobalErrorHandlerExtension
{
    public static WebApplicationBuilder AddGlobalErrorHandler(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

        return builder;
    }
}
