namespace Hr.Api.Configurations.Extensions;

public static class WebApplicationBuilderAddHttpClientExtension
{
    public static WebApplicationBuilder AddHttpClient(this WebApplicationBuilder builder)
    {
        // skip SSL error (for demo purposes only!)
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
            {
                Console.WriteLine("SSL error skipped");
                return true;
            }
        };

        var externalApiBaseUrl = builder.Configuration["ExternalApiBaseUrl"];

        builder.Services.AddHttpClient("MyCustomClient", client =>
        {
            client.BaseAddress = new Uri(externalApiBaseUrl!);
        })
        .ConfigurePrimaryHttpMessageHandler(() => handler);

        return builder;
    }
}