using System.Text.Json.Serialization;

namespace Infrastructure.ExternalServices.AI.Models
{
    internal record GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<Candidate>? Candidates { get; init; }
    }

    internal record Candidate
    {
        [JsonPropertyName("content")]
        public ResponseContent? Content { get; init; }
    }

    internal record ResponseContent
    {
        [JsonPropertyName("parts")]
        public List<ResponsePart>? Parts { get; init; }
    }

    internal record ResponsePart
    {
        [JsonPropertyName("text")]
        public string? Text { get; init; }
    }
}
