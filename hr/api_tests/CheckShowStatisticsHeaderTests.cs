using Hr.Api.Configurations.Middleware.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hr.Api.Tests;

public class CheckShowStatisticsHeaderTests
{
    [Fact]
    public void OnActionExecuting_InvokeWithoutShowStatisticsHeader_ReturnsBadRequest()
    {
        // Arrange 
        var checkShowStatisticsHeaderActionFilter =
            new CheckShowStatisticsHeader();

        var httpContext = new DefaultHttpContext();

        var actionContext = new ActionContext(httpContext, new(), new(), new());
        var actionExecutingContext = new ActionExecutingContext(actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            controller: default!);

        // Act
        checkShowStatisticsHeaderActionFilter.OnActionExecuting(actionExecutingContext);

        // Assert
        Assert.IsType<BadRequestResult>(actionExecutingContext.Result);
    }
}