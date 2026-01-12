namespace Application.Common.Abstractions.ExternalServices.AI
{
    public interface IAIService
    {
        string ProviderName { get; }

        Task<string> GenerateContentAsync(string prompt, CancellationToken cancellationToken);

        Task<string> GenerateContentAsync(
            string systemPrompt,
            string userPrompt,
            CancellationToken cancellationToken);
    }
}