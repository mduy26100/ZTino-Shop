using Application.Common.Models.Responses;
using System.Text.Json;

namespace WebAPI.Middleware.Response
{
    public class ResponseWrappingMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseWrappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBody = context.Response.Body;

            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            try
            {
                await _next(context);

                if (context.Response.HasStarted)
                    return;

                var statusCode = context.Response.StatusCode;

                if (statusCode < 200 || statusCode >= 300)
                {
                    newBody.Seek(0, SeekOrigin.Begin);
                    await newBody.CopyToAsync(originalBody);
                    return;
                }

                if (statusCode == StatusCodes.Status204NoContent)
                {
                    context.Response.Body = originalBody;
                    context.Response.ContentLength = 0;
                    return;
                }

                newBody.Seek(0, SeekOrigin.Begin);
                var bodyText = await new StreamReader(newBody).ReadToEndAsync();

                object? data = string.IsNullOrWhiteSpace(bodyText)
                    ? null
                    : JsonSerializer.Deserialize<object>(bodyText);

                var meta = new Meta
                {
                    Timestamp = DateTime.UtcNow,
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    StatusCode = statusCode,

                    TraceId = context.TraceIdentifier,
                    RequestId = context.Items["RequestId"]?.ToString(),
                    ClientIp = context.Connection.RemoteIpAddress?.ToString()
                };

                var responseWrapped = ApiResponse.Success(data, meta);

                var json = JsonSerializer.Serialize(
                    responseWrapped,
                    new JsonSerializerOptions { WriteIndented = true }
                );

                context.Response.ContentType = "application/json";
                context.Response.Body = originalBody;

                await context.Response.WriteAsync(json);
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}
