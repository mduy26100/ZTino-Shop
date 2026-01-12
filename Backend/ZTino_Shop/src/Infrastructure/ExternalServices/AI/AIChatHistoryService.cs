using Application.Common.Abstractions.Caching;
using Application.Common.Abstractions.ExternalServices.AI;
using Application.Features.AI.v1.DTOs;
using Application.Features.AI.v1.Services;

namespace Infrastructure.ExternalServices.AI
{
    public class AIChatHistoryService : IAIChatHistoryService
    {
        private readonly ICacheService _cache;
        private readonly IAIFactory _aiFactory;
        private const int MaxHistoryMessages = 4;
        private static readonly TimeSpan SessionExpiry = TimeSpan.FromMinutes(30);

        public AIChatHistoryService(ICacheService cache, IAIFactory aiFactory)
        {
            _cache = cache;
            _aiFactory = aiFactory;
        }

        public async Task<List<ChatMessage>> GetRecentHistoryAsync(
            string sessionId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(sessionId))
                return new List<ChatMessage>();

            var cacheKey = GetCacheKey(sessionId);
            var session = await _cache.GetAsync<ChatSession>(cacheKey, cancellationToken);

            return session?.Messages
                .OrderByDescending(m => m.Timestamp)
                .Take(MaxHistoryMessages)
                .OrderBy(m => m.Timestamp)
                .ToList() ?? new List<ChatMessage>();
        }

        public async Task SaveMessageAsync(
            string sessionId,
            ChatMessage message,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(sessionId))
                return;

            var cacheKey = GetCacheKey(sessionId);
            var session = await _cache.GetAsync<ChatSession>(cacheKey, cancellationToken)
                ?? new ChatSession { SessionId = sessionId };

            session.Messages.Add(message);

            if (session.Messages.Count > MaxHistoryMessages * 2)
            {
                session.Messages = session.Messages
                    .OrderByDescending(m => m.Timestamp)
                    .Take(MaxHistoryMessages * 2)
                    .OrderBy(m => m.Timestamp)
                    .ToList();
            }

            await _cache.SetAsync(cacheKey, session, SessionExpiry, cancellationToken);
        }

        public async Task<string> RewriteQueryWithContextAsync(
            string currentPrompt,
            List<ChatMessage> history,
            CancellationToken cancellationToken = default)
        {
            if (!history.Any() || IsSpecificQuery(currentPrompt))
                return currentPrompt;

            var rewritePrompt = BuildRewritePrompt(currentPrompt, history);

            var aiService = await _aiFactory.GetActiveServiceAsync(cancellationToken);
            var rewritten = await aiService.GenerateContentAsync(rewritePrompt, cancellationToken);

            return CleanRewrittenQuery(rewritten, currentPrompt);
        }

        private static bool IsSpecificQuery(string prompt)
        {
            var specificIndicators = new[]
            {
                "polo", "shirt", "jeans", "cardigan", "blazer", "hoodie",
                "áo", "quần", "sản phẩm", "màu", "size"
            };

            var lowerPrompt = prompt.ToLowerInvariant();
            return specificIndicators.Count(i => lowerPrompt.Contains(i)) >= 2;
        }

        private static string BuildRewritePrompt(string currentPrompt, List<ChatMessage> history)
        {
            var historyText = string.Join("\n",
                history.Select(m => $"{m.Role}: {m.Content}"));

            return $"""
                Bạn là một trợ lý viết lại câu hỏi. Nhiệm vụ của bạn là:
                
                1. Đọc lịch sử hội thoại bên dưới
                2. Đọc câu hỏi mới nhất của người dùng
                3. Viết lại câu hỏi mới thành một câu ĐẦY ĐỦ NGỮ NGHĨA (không cần context để hiểu)
                
                ## Lịch sử hội thoại:
                {historyText}
                
                ## Câu hỏi mới:
                {currentPrompt}
                
                ## Quy tắc:
                - Nếu câu hỏi mới đã đầy đủ ngữ nghĩa, trả về nguyên văn
                - Nếu câu hỏi mới có đại từ tham chiếu (nó, đó, cái này, that, it), thay thế bằng từ cụ thể từ lịch sử
                - CHỈ trả về câu hỏi đã viết lại, KHÔNG giải thích
                
                ## Ví dụ:
                Lịch sử: "user: Cardigan Regular có màu gì?"
                Câu mới: "Còn size gì?"
                Kết quả: "Cardigan Regular còn size gì?"
                
                ## Kết quả (chỉ câu hỏi):
                """;
        }

        private static string CleanRewrittenQuery(string rewritten, string fallback)
        {
            if (string.IsNullOrWhiteSpace(rewritten))
                return fallback;

            rewritten = rewritten.Trim().Trim('"', '\'', '`');

            if (rewritten.Length > 200 || rewritten.Contains('\n'))
                return fallback;

            return rewritten;
        }

        private static string GetCacheKey(string sessionId) => $"ai:chat:{sessionId}";
    }
}
