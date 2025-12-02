namespace Application.Common.Models.Responses
{
    public class Meta
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Path { get; set; } = "";
    }
}
