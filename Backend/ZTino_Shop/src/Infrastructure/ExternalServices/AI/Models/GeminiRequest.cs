using System.Text.Json.Serialization;

namespace Infrastructure.ExternalServices.AI.Models
{
    internal record GeminiRequest
    {
        [JsonPropertyName("contents")]
        public List<RequestContent> Contents { get; init; } = new();
    }

    internal record RequestContent
    {
        [JsonPropertyName("parts")]
        public List<RequestPart> Parts { get; init; } = new();
    }

    internal record RequestPart
    {
        [JsonPropertyName("text")]
        public string Text { get; init; } = string.Empty;
    }
}