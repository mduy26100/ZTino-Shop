using Application.Common.Exceptions;
using WebAPI.Responses;
using System.Net;
using System.Text.Json;

namespace WebAPI.Middleware.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false 
        };

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (!context.Response.HasStarted)
                {
                    switch (context.Response.StatusCode)
                    {
                        case StatusCodes.Status401Unauthorized:
                            await WriteErrorResponse(context, HttpStatusCode.Unauthorized, new ApiError { Type = "unauthorized", Message = "Authentication required." });
                            break;
                        case StatusCodes.Status403Forbidden:
                            await WriteErrorResponse(context, HttpStatusCode.Forbidden, new ApiError { Type = "forbidden", Message = "Access denied." });
                            break;
                        case StatusCodes.Status404NotFound:
                            await WriteErrorResponse(context, HttpStatusCode.NotFound, new ApiError { Type = "not-found", Message = "Resource or Endpoint not found." });
                            break;
                        case StatusCodes.Status405MethodNotAllowed:
                            await WriteErrorResponse(context, HttpStatusCode.MethodNotAllowed, new ApiError { Type = "method-not-allowed", Message = $"HTTP Method {context.Request.Method} is not allowed for this endpoint." });
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started, the exception middleware will not be executed.");
                return;
            }

            HttpStatusCode statusCode;
            ApiError apiError;

            switch (ex)
            {
                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    apiError = new ApiError
                    {
                        Type = "validation-error",
                        Message = "Validation failed.",
                        Details = validationEx.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                    };
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    apiError = new ApiError { Type = "unauthorized", Message = ex.Message };
                    break;
                
                case ForbiddenException:
                    statusCode = HttpStatusCode.Forbidden;
                    apiError = new ApiError { Type = "forbidden", Message = ex.Message };
                    break;

                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    apiError = new ApiError { Type = "not-found", Message = ex.Message };
                    break;

                case ConflictException:
                    statusCode = HttpStatusCode.Conflict;
                    apiError = new ApiError { Type = "conflict", Message = ex.Message };
                    break;

                case BusinessRuleException:
                    statusCode = HttpStatusCode.BadRequest;
                    apiError = new ApiError { Type = "business-rule-violation", Message = ex.Message };
                    break;

                default:
                    _logger.LogError(ex, "Unhandled Exception");
                    statusCode = HttpStatusCode.InternalServerError;
                    apiError = new ApiError { Type = "internal-server-error", Message = "An internal error occurred." };
                    break;
            }

            await WriteErrorResponse(context, statusCode, apiError);
        }

        private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, ApiError error)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var meta = new Meta
            {
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path,
                Method = context.Request.Method,
                StatusCode = (int)statusCode,
                TraceId = context.TraceIdentifier,
                RequestId = context.Items["RequestId"]?.ToString(),
                ClientIp = context.Connection.RemoteIpAddress?.ToString()
            };

            var response = ApiResponse.Fail(error);
            response.Meta = meta;

            var json = JsonSerializer.Serialize(response, JsonOptions);

            await context.Response.WriteAsync(json);
        }
    }
}
