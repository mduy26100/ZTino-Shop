using System.Text.Json;
using System.Threading.RateLimiting;
using WebAPI.Responses;

namespace WebAPI.DependencyInjection.CrossCutting
{
    public static class RateLimitingExtensions
    {
        public static class Policies
        {
            public const string IpLimit = "IpRateLimit";
        }

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public static IServiceCollection AddRateLimitingConfig(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (context, token) =>
                {
                    var httpContext = context.HttpContext;
                    httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    httpContext.Response.ContentType = "application/json";

                    string message = "Too many requests. Please try again later.";

                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        message = $"Too many requests. Please try again after {retryAfter.TotalSeconds} seconds.";
                        httpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString();
                    }

                    var meta = new Meta
                    {
                        Timestamp = DateTime.UtcNow,
                        Path = httpContext.Request.Path,
                        Method = httpContext.Request.Method,
                        StatusCode = StatusCodes.Status429TooManyRequests,
                        TraceId = httpContext.TraceIdentifier,
                        RequestId = httpContext.Items["RequestId"]?.ToString(),
                        ClientIp = httpContext.Connection.RemoteIpAddress?.ToString()
                    };

                    var apiError = new ApiError
                    {
                        Type = "rate-limit-exceeded",
                        Message = message
                    };

                    var response = ApiResponse.Fail(apiError);
                    response.Meta = meta;

                    var json = JsonSerializer.Serialize(response, _jsonOptions);
                    await httpContext.Response.WriteAsync(json, token);
                };

                options.AddPolicy(Policies.IpLimit, httpContext =>
                {
                    var remoteIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetTokenBucketLimiter(
                        partitionKey: remoteIp,
                        factory: _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = 10,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                            TokensPerPeriod = 2,
                            AutoReplenishment = true
                        });
                });
            });

            return services;
        }
    }
}