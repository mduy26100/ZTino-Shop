using System.Text.Json.Serialization;

namespace Infrastructure.ExternalServices.AI.Models
{
    internal record GeminiRequest
    {
        [JsonPropertyName("system_instruction")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SystemInstruction? SystemInstruction { get; init; }

        [JsonPropertyName("contents")]
        public List<RequestContent> Contents { get; init; } = new();
    }

    internal record SystemInstruction
    {
        [JsonPropertyName("parts")]
        public List<RequestPart> Parts { get; init; } = new();
    }

    internal record RequestContent
    {
        [JsonPropertyName("role")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Role { get; init; }

        [JsonPropertyName("parts")]
        public List<RequestPart> Parts { get; init; } = new();
    }

    internal record RequestPart
    {
        [JsonPropertyName("text")]
        public string Text { get; init; } = string.Empty;
    }
}
