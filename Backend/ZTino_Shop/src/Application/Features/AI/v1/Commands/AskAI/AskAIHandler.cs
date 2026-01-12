using Application.Common.Abstractions.ExternalServices.AI;
using Application.Features.AI.v1.DTOs;
using Application.Features.AI.v1.Services;
using System.Text.Json;

namespace Application.Features.AI.v1.Commands.AskAI
{
    public class AskAIHandler : IRequestHandler<AskAICommand, string>
    {
        private readonly IAIFactory _aiFactory;
        private readonly IAIContextService _contextService;
        private readonly IAIChatHistoryService _chatHistoryService;

        public AskAIHandler(
            IAIFactory aiFactory,
            IAIContextService contextService,
            IAIChatHistoryService chatHistoryService)
        {
            _aiFactory = aiFactory;
            _contextService = contextService;
            _chatHistoryService = chatHistoryService;
        }

        public async Task<string> Handle(AskAICommand request, CancellationToken cancellationToken)
        {
            var sessionId = request.SessionId ?? Guid.NewGuid().ToString();

            var history = await _chatHistoryService.GetRecentHistoryAsync(sessionId, cancellationToken);

            var processedPrompt = await _chatHistoryService.RewriteQueryWithContextAsync(
                request.Prompt, history, cancellationToken);

            var keywords = AIKeywordExtractor.Extract(processedPrompt);

            var intent = AIKeywordExtractor.ClassifyIntent(processedPrompt, keywords);

            string systemPrompt = intent switch
            {
                UserIntent.Chitchat => BuildChitchatPrompt(),
                UserIntent.Discovery => await BuildDiscoveryPromptAsync(cancellationToken),
                UserIntent.ProductSearch => await BuildProductSearchPromptAsync(keywords, processedPrompt, cancellationToken),
                _ => BuildChitchatPrompt()
            };

            var aiService = await _aiFactory.GetActiveServiceAsync(cancellationToken);
            var response = await aiService.GenerateContentAsync(
                systemPrompt,
                request.Prompt,
                cancellationToken);

            await SaveChatHistoryAsync(sessionId, request.Prompt, response, cancellationToken);

            return response;
        }

        private async Task SaveChatHistoryAsync(string sessionId, string userMessage, string aiResponse, CancellationToken ct)
        {
            await _chatHistoryService.SaveMessageAsync(sessionId, new ChatMessage
            {
                Role = "user",
                Content = userMessage
            }, ct);

            await _chatHistoryService.SaveMessageAsync(sessionId, new ChatMessage
            {
                Role = "assistant",
                Content = aiResponse
            }, ct);
        }

        private static string BuildChitchatPrompt()
        {
            return """
                Bạn là trợ lý ảo của ZTino - thương hiệu thời trang nam cao cấp tại Việt Nam.
                
                ## CHẾ ĐỘ TRÒ CHUYỆN:
                Khách đang chào hỏi hoặc hỏi thông tin chung. Trả lời thân thiện!
                
                ## CÁCH TRẢ LỜI:
                1. Chào đón nhiệt tình, xưng "em" với khách
                2. Tự giới thiệu ngắn gọn về ZTino (thời trang nam cao cấp)
                3. Hỏi khách cần tư vấn sản phẩm gì cụ thể
                4. KHÔNG bịa thông tin về sản phẩm, giá, tồn kho
                5. Phát hiện ngôn ngữ: Việt→Việt, Anh→Anh
                
                ## VÍ DỤ:
                Khách: "Chào shop"
                Bạn: "Chào anh/chị! Em là trợ lý của ZTino - thương hiệu thời trang nam cao cấp. 
                      Anh/chị đang tìm kiếm sản phẩm gì ạ? Em có thể tư vấn Áo Polo, Sơ mi, Quần Âu..."
                """;
        }

        private async Task<string> BuildDiscoveryPromptAsync(CancellationToken ct)
        {
            var discovery = await _contextService.GetDiscoveryContextAsync(ct);
            var categories = string.Join(", ", discovery.Categories);

            return $"""
                Bạn là trợ lý của ZTino - thương hiệu thời trang nam cao cấp tại Việt Nam.
                
                ## CHẾ ĐỘ GIỚI THIỆU:
                Khách đang tìm hiểu shop bán gì. Giới thiệu tổng quan!
                
                ## DANH MỤC SẢN PHẨM:
                {categories}
                
                ## CÁCH TRẢ LỜI:
                1. Giới thiệu ZTino chuyên thời trang nam cao cấp
                2. Liệt kê các danh mục sản phẩm đang có
                3. Hỏi khách quan tâm dòng sản phẩm nào để tư vấn chi tiết
                4. KHÔNG nói chi tiết giá/size/tồn kho (chưa hỏi cụ thể)
                5. Phát hiện ngôn ngữ: Việt→Việt, Anh→Anh
                """;
        }

        private async Task<string> BuildProductSearchPromptAsync(
            ExtractedKeywords keywords, string prompt, CancellationToken ct)
        {
            var products = await _contextService.GetProductContextAsync(keywords, ct);

            var priceRange = AIKeywordExtractor.ExtractPriceRange(prompt);
            if (priceRange.MinPrice.HasValue || priceRange.MaxPrice.HasValue)
            {
                products = products.Where(p =>
                    (!priceRange.MinPrice.HasValue || p.MaxPrice >= priceRange.MinPrice) &&
                    (!priceRange.MaxPrice.HasValue || p.MinPrice <= priceRange.MaxPrice))
                    .ToList();
            }

            return BuildProductSystemPrompt(products);
        }

        private static string BuildProductSystemPrompt(List<AIProductContextDto> products)
        {
            var basePrompt = """
                Bạn là trợ lý tư vấn bán hàng của ZTino - thương hiệu thời trang nam cao cấp.
                
                ## THÔNG TIN QUAN TRỌNG:
                - Dữ liệu sản phẩm được lưu bằng TIẾNG ANH
                - Khách có thể hỏi bằng TIẾNG VIỆT hoặc ANH
                - Bạn cần match thông minh giữa 2 ngôn ngữ
                
                ## QUY TẮC BẮT BUỘC:
                1. CHỈ trả lời dựa trên dữ liệu sản phẩm bên dưới
                2. Nêu rõ: Tên, Màu, Size còn hàng, Giá, Tình trạng
                3. "In Stock" = Còn hàng, "Low Stock" = Sắp hết, "Out of Stock" = Hết
                4. TUYỆT ĐỐI KHÔNG bịa sản phẩm hoặc số lượng tồn kho
                5. Nếu không tìm thấy: "Shop chưa có sản phẩm này. Liên hệ hotline nhé!"
                6. Phát hiện ngôn ngữ: Việt→Việt, Anh→Anh
                
                ## DỮ LIỆU SẢN PHẨM:
                """;

            if (!products.Any())
                return basePrompt + "\n[Không tìm thấy sản phẩm phù hợp]";

            var productInfo = JsonSerializer.Serialize(products, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            return $"{basePrompt}\n```json\n{productInfo}\n```";
        }
    }
}
