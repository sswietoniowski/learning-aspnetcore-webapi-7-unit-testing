using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hr.Api.Configurations.Middleware.Filters;

public class CheckShowStatisticsHeader : ActionFilterAttribute
{
    private readonly string _showStatisticsHeader = "ShowStatistics";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // if the ShowStatistics header is missing or set to false, 
        // a BadRequest must be returned.
        if (!context.HttpContext.Request.Headers.ContainsKey(_showStatisticsHeader))
        {
            context.Result = new BadRequestResult();
        }

        // get the ShowStatistics header 
        if (!bool.TryParse(context.HttpContext.Request.Headers[_showStatisticsHeader].ToString(), 
            out bool showStatisticsValue))
        {
            context.Result = new BadRequestResult();
        }

        // check the value
        if (!showStatisticsValue)
        {
            context.Result = new BadRequestResult();
        }
    } 
}