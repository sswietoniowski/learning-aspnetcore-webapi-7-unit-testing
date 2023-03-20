namespace Hr.Api.Configurations.Middleware;

public class SecurityHeadersMiddleware : IMiddleware
{
    private readonly ILogger<SecurityHeadersMiddleware> _logger;

    public SecurityHeadersMiddleware(ILogger<SecurityHeadersMiddleware> logger)
    {
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        IHeaderDictionary headers = context.Request.Headers;

        // Add CSP + X-Content-Type
        headers["Content-Security-Policy"] = "default-src 'self';frame-ancestors 'none';"; 
        headers["X-Content-Type-Options"] = "nosniff"; 

        await next(context);
    }
}