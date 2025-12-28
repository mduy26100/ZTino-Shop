using Application.Common.Models.Responses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters.Response;

public class ApiResponseFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult)
        {
            if (objectResult.Value == null || 
                objectResult.Value is ApiResponse || 
                objectResult.Value is ProblemDetails)
            {
                await next();
                return;
            }

            var httpContext = context.HttpContext;
            var meta = new Meta
            {
                Timestamp = DateTime.UtcNow,
                Path = httpContext.Request.Path,
                Method = httpContext.Request.Method,
                StatusCode = objectResult.StatusCode ?? 200,
                TraceId = httpContext.TraceIdentifier,
                RequestId = httpContext.Items["RequestId"]?.ToString(),
                ClientIp = httpContext.Connection.RemoteIpAddress?.ToString()
            };

            var responseWrapped = ApiResponse.Success(objectResult.Value, meta);
            
            objectResult.Value = responseWrapped;
        }

        await next();
    }
}