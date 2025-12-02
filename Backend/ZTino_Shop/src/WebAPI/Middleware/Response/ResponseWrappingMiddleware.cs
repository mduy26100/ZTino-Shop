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

            try
            {
                using var newBody = new MemoryStream();
                context.Response.Body = newBody;

                await _next(context);

                if (context.Response.HasStarted)
                {
                    return;
                }

                int statusCode = context.Response.StatusCode;

                if (statusCode == StatusCodes.Status204NoContent)
                {
                    context.Response.Body = originalBody;
                    context.Response.ContentLength = 0;
                    return;
                }

                newBody.Seek(0, SeekOrigin.Begin);
                string bodyText = await new StreamReader(newBody).ReadToEndAsync();
                object? data;

                try
                {
                    data = string.IsNullOrWhiteSpace(bodyText)
                        ? null
                        : JsonSerializer.Deserialize<object>(bodyText);
                }
                catch
                {
                    data = bodyText;
                }

                var meta = new Meta
                {
                    Timestamp = DateTime.UtcNow,
                    Path = context.Request.Path
                };

                ApiResponse responseWrapped;

                if (statusCode >= 400)
                {
                    var error = new ApiError
                    {
                        Type = statusCode.ToString(),
                        Message = string.IsNullOrWhiteSpace(bodyText)
                            ? "An error occurred."
                            : bodyText,
                        Details = null
                    };

                    responseWrapped = ApiResponse.Fail(error, statusCode);
                }
                else
                {
                    responseWrapped = ApiResponse.Success(data, statusCode, meta);
                }

                string json = JsonSerializer.Serialize(
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
