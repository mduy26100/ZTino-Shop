using WebAPI.Responses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters.Response;

public class ApiResponseFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is EmptyResult)
        {
            context.Result = new ObjectResult(null) { StatusCode = 200 };
        }

        if (context.Result is StatusCodeResult statusCodeResult && statusCodeResult.StatusCode != 204)
        {
            context.Result = new ObjectResult(null) { StatusCode = statusCodeResult.StatusCode };
        }

        if (context.Result is ObjectResult objectResult)
        {
            if (objectResult.StatusCode == 204)
            {
                await next();
                return;
            }

            if (objectResult.Value is ApiResponse || objectResult.Value is ProblemDetails)
            {
                await next();
                return;
            }

            var statusCode = objectResult.StatusCode ?? 200;
            var httpContext = context.HttpContext;

            var meta = new Meta
            {
                Timestamp = DateTime.UtcNow,
                Path = httpContext.Request.Path,
                Method = httpContext.Request.Method,
                StatusCode = statusCode,
                TraceId = httpContext.TraceIdentifier,
                RequestId = httpContext.Items["RequestId"]?.ToString(),
                ClientIp = httpContext.Connection.RemoteIpAddress?.ToString()
            };

            var responseWrapped = ApiResponse.Success(objectResult.Value, meta);
            
            objectResult.Value = responseWrapped;
            
            objectResult.DeclaredType = typeof(ApiResponse); 
        }

        await next();
    }
}
