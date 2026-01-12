using Application.Features.AI.v1.DTOs;

namespace Application.Features.AI.v1.Services
{
    public interface IAIChatHistoryService
    {
        Task<List<ChatMessage>> GetRecentHistoryAsync(
            string sessionId,
            CancellationToken cancellationToken = default);

        Task SaveMessageAsync(
            string sessionId,
            ChatMessage message,
            CancellationToken cancellationToken = default);

        Task<string> RewriteQueryWithContextAsync(
            string currentPrompt,
            List<ChatMessage> history,
            CancellationToken cancellationToken = default);
    }
}
