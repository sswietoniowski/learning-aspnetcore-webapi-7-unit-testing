using Hr.Api.Configurations.Middleware;
using Microsoft.AspNetCore.Http;

namespace Hr.Api.Tests;

public class EmployeeManagementSecurityHeadersMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_Invoke_SetsExpectedResponseHeaders()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        RequestDelegate next = (HttpContext httpContext) => Task.CompletedTask;

        var middleware = new SecurityHeadersMiddleware(default!);

        // Act
        await middleware.InvokeAsync(httpContext, next);

        // Assert
        var cspHeader = httpContext.Response.Headers["Content-Security-Policy"].ToString();
        var xContentTypeOptionsHeader = httpContext.Response.Headers["X-Content-Type-Options"].ToString();

        Assert.Equal("default-src 'self';frame-ancestors 'none';", cspHeader);
        Assert.Equal("nosniff", xContentTypeOptionsHeader);
    }
}