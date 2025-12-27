using Application.Common.Exceptions;
using Application.Common.Models.Responses;
using System.Net;
using System.Text.Json;

namespace WebAPI.Middleware.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

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
            }
            catch (ValidationException ex)
            {
                await WriteErrorResponse(context, HttpStatusCode.BadRequest, new ApiError
                {
                    Type = "validation-error",
                    Message = "Validation failed.",
                    Details = ex.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                await WriteErrorResponse(context, HttpStatusCode.Unauthorized, new ApiError
                {
                    Type = "unauthorized",
                    Message = ex.Message,
                    Details = null
                });
            }
            catch (NotFoundException ex)
            {
                await WriteErrorResponse(context, HttpStatusCode.NotFound, new ApiError
                {
                    Type = "not-found",
                    Message = ex.Message,
                    Details = null
                });
            }
            catch (ConflictException ex)
            {
                await WriteErrorResponse(context, HttpStatusCode.Conflict, new ApiError
                {
                    Type = "conflict",
                    Message = ex.Message,
                    Details = null
                });
            }
            catch (ForbiddenException ex)
            {
                await WriteErrorResponse(context, HttpStatusCode.Forbidden, new ApiError
                {
                    Type = "forbidden",
                    Message = ex.Message,
                    Details = null
                });
            }
            catch (BusinessRuleException ex)
            {
                await WriteErrorResponse(context, HttpStatusCode.BadRequest, new ApiError
                {
                    Type = "business-rule-violation",
                    Message = ex.Message,
                    Details = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception");

                await WriteErrorResponse(context, HttpStatusCode.InternalServerError, new ApiError
                {
                    Type = "internal-server-error",
                    Message = ex.Message,
                    Details = null
                });
            }
        }

        private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, ApiError error)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var meta = new Meta
            {
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path
            };

            var responseWrapped = ApiResponse.Fail(error, (int)statusCode);
            responseWrapped.Meta = meta;

            string json = JsonSerializer.Serialize(responseWrapped, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await context.Response.WriteAsync(json);
        }
    }
}
