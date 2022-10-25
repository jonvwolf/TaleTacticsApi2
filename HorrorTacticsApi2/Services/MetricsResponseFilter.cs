using HorrorTacticsApi2.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HorrorTacticsApi2.Services
{
    public class MetricsResponseFilter : IResultFilter
    {
        readonly MetricsService metricsService;
        public MetricsResponseFilter(MetricsService service)
        {
            metricsService = service;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            var userId = HtController.GetUserId(context.HttpContext);

            metricsService.AddRequest(new RequestModel(
                DateTimeOffset.Now, 
                context.HttpContext.Request.Path, 
                context.HttpContext.Request.Method, 
                userId, 
                context.HttpContext.Response.StatusCode));
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            
        }
    }
}
