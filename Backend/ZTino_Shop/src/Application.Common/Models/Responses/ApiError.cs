namespace Application.Common.Models.Responses
{
    public class ApiError
    {
        public string? Type { get; set; }
        public string? Message { get; set; }
        public object? Details { get; set; }
    }
}
