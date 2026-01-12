namespace Application.Features.AI.v1.DTOs
{
    public class ChatMessage
    {
        public string Role { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ChatSession
    {
        public string SessionId { get; set; } = default!;
        public List<ChatMessage> Messages { get; set; } = new();
    }
}
