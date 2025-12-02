namespace Application.Common.Models.Responses
{
    public class ApiResponse
    {
        public object? Data { get; set; }
        public object? Meta { get; set; } = new Meta();
        public ApiError? Error { get; set; }
        public int StatusCode { get; set; }

        public static ApiResponse Success(object? data, int statusCode, object? meta = null)
            => new ApiResponse
            {
                StatusCode = statusCode,
                Data = data,
                Meta = meta ?? new Meta(),
                Error = null
            };

        public static ApiResponse Fail(ApiError error, int statusCode)
            => new ApiResponse
            {
                StatusCode = statusCode,
                Data = null,
                Meta = new Meta(),
                Error = error,
            };
    }
}
