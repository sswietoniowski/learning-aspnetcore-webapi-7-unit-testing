using Microsoft.OpenApi.Models;

namespace Hr.Api.Configurations.Extensions;

public static class WebApplicationBuilderAddSwaggerExtension
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Management.Api", Version = "v1" });
        });

        return builder;
    }
}