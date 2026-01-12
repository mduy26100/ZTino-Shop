using System.Text.RegularExpressions;

namespace Application.Features.AI.v1.Services
{
    public static class AIKeywordExtractor
    {
        private static readonly HashSet<string> CommonPhrases = new(StringComparer.OrdinalIgnoreCase)
        {
            // Shirt types
            "áo sơ mi", "áo thun", "áo polo", "áo khoác", "áo len", "áo vest",
            "áo hoodie", "áo cardigan", "áo blazer", "áo gile",
            // Pants types  
            "quần jean", "quần jeans", "quần âu", "quần tây", "quần kaki",
            "quần short", "quần jogger", "quần dài",
            // Colors in Vietnamese
            "màu đen", "màu trắng", "màu xanh", "màu đỏ", "màu xám",
            "màu be", "màu nâu", "màu navy", "màu xanh dương", "màu xanh lá",
            // Other
            "sắp hết", "còn hàng", "hết hàng", "giá rẻ"
        };

        private static readonly Dictionary<string, string> ViToEnMap = new(StringComparer.OrdinalIgnoreCase)
        {
            // Shirt types
            ["áo sơ mi"] = "shirt",
            ["áo thun"] = "t-shirt",
            ["áo polo"] = "polo",
            ["áo khoác"] = "jacket",
            ["áo len"] = "sweater",
            ["áo vest"] = "vest",
            ["áo hoodie"] = "hoodie",
            ["áo cardigan"] = "cardigan",
            ["áo blazer"] = "blazer",
            ["áo gile"] = "gilet",
            // Pants types
            ["quần jean"] = "jeans",
            ["quần jeans"] = "jeans",
            ["quần âu"] = "trousers",
            ["quần tây"] = "trousers",
            ["quần kaki"] = "chinos",
            ["quần short"] = "shorts",
            ["quần jogger"] = "jogger",
            ["quần dài"] = "pants",
            // Colors
            ["đen"] = "black",
            ["trắng"] = "white",
            ["xanh"] = "blue",
            ["xanh dương"] = "blue",
            ["xanh lá"] = "green",
            ["đỏ"] = "red",
            ["xám"] = "gray",
            ["be"] = "beige",
            ["nâu"] = "brown",
            ["navy"] = "navy",
            ["hồng"] = "pink",
            ["vàng"] = "yellow",
            ["tím"] = "purple",
            ["cam"] = "orange",
            ["kem"] = "cream",
            // Adjectives
            ["màu"] = "",
        };

        private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "tôi", "muốn", "mua", "xem", "cho", "cái", "chiếc", "có", "không", "được",
            "là", "và", "hoặc", "của", "với", "trong", "ngoài", "trên", "dưới",
            "bạn", "shop", "ztino", "hàng", "sản", "phẩm", "giá", "bao", "nhiêu",
            "i", "want", "to", "buy", "see", "show", "me", "the", "a", "an",
            "còn", "nào", "nữa", "đi", "nhé", "ạ", "vậy", "thì", "mà", "như",
            "this", "that", "these", "those", "please", "can", "you", "your"
        };

        private static readonly Regex SizePattern = new(
            @"\b(XS|S|M|L|XL|XXL|XXXL|2XL|3XL|4XL|5XL|\d{2})\b",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex QuotedPattern = new(
            "\"([^\"]+)\"|'([^']+)'",
            RegexOptions.Compiled);

        public static ExtractedKeywords Extract(string prompt)
        {
            var result = new ExtractedKeywords();

            if (string.IsNullOrWhiteSpace(prompt))
                return result;

            var workingPrompt = prompt.ToLowerInvariant();
            var extractedPhrases = new List<string>();

            var quotedMatches = QuotedPattern.Matches(prompt);
            foreach (Match match in quotedMatches)
            {
                var quoted = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value;
                extractedPhrases.Add(quoted.Trim());
                workingPrompt = workingPrompt.Replace(match.Value.ToLowerInvariant(), " ");
            }

            foreach (var phrase in CommonPhrases.OrderByDescending(p => p.Length))
            {
                if (workingPrompt.Contains(phrase.ToLowerInvariant()))
                {
                    extractedPhrases.Add(phrase);
                    workingPrompt = workingPrompt.Replace(phrase.ToLowerInvariant(), " ");
                }
            }

            var sizeMatches = SizePattern.Matches(workingPrompt);
            foreach (Match match in sizeMatches)
            {
                result.Sizes.Add(match.Value.ToUpperInvariant());
                workingPrompt = workingPrompt.Replace(match.Value.ToLowerInvariant(), " ");
            }

            var remainingWords = Regex.Split(workingPrompt, @"[\s,;.!?]+")
                .Where(w => w.Length >= 2)
                .Where(w => !StopWords.Contains(w))
                .ToList();

            extractedPhrases.AddRange(remainingWords);

            foreach (var phrase in extractedPhrases.Distinct())
            {
                result.OriginalKeywords.Add(phrase);

                if (ViToEnMap.TryGetValue(phrase, out var englishTerm) && !string.IsNullOrEmpty(englishTerm))
                {
                    result.TranslatedKeywords.Add(englishTerm);
                }
                else
                {
                    result.TranslatedKeywords.Add(phrase);
                }
            }

            return result;
        }

        public static (decimal? MinPrice, decimal? MaxPrice) ExtractPriceRange(string prompt)
        {
            var underPattern = new Regex(@"(?:dưới|under|below|<)\s*(\d+)k?", RegexOptions.IgnoreCase);
            var abovePattern = new Regex(@"(?:trên|above|over|>)\s*(\d+)k?", RegexOptions.IgnoreCase);
            var rangePattern = new Regex(@"(?:từ|from)\s*(\d+)k?\s*(?:đến|to|-)\s*(\d+)k?", RegexOptions.IgnoreCase);

            var rangeMatch = rangePattern.Match(prompt);
            if (rangeMatch.Success)
            {
                return (ParsePrice(rangeMatch.Groups[1].Value), ParsePrice(rangeMatch.Groups[2].Value));
            }

            decimal? min = null, max = null;

            var underMatch = underPattern.Match(prompt);
            if (underMatch.Success)
                max = ParsePrice(underMatch.Groups[1].Value);

            var aboveMatch = abovePattern.Match(prompt);
            if (aboveMatch.Success)
                min = ParsePrice(aboveMatch.Groups[1].Value);

            return (min, max);
        }

        private static decimal ParsePrice(string value)
        {
            if (decimal.TryParse(value.Replace(".", "").Replace(",", ""), out var price))
                return price < 1000 ? price * 1000 : price;
            return 0;
        }

        private static readonly HashSet<string> DiscoveryTriggers = new(StringComparer.OrdinalIgnoreCase)
        {
            "bán gì", "có gì", "đồ nam", "quần áo", "thời trang",
            "danh mục", "sản phẩm", "collection", "menu", "có những gì",
            "bán những gì", "loại nào", "dòng nào", "mẫu nào"
        };

        private static readonly HashSet<string> ProductEntities = new(StringComparer.OrdinalIgnoreCase)
        {
            // Product types (Vietnamese)
            "polo", "sơ mi", "thun", "hoodie", "cardigan", "blazer", "vest", "khoác",
            "jean", "jeans", "âu", "tây", "kaki", "short", "jogger",
            // Product types (English)
            "shirt", "t-shirt", "jacket", "sweater", "trousers", "pants", "chinos",
            // Colors (Vietnamese)
            "đen", "trắng", "xanh", "đỏ", "xám", "be", "nâu", "navy", "hồng", "vàng", "tím", "cam", "kem",
            // Colors (English)
            "black", "white", "blue", "red", "gray", "beige", "brown", "pink", "yellow", "purple", "orange"
        };

        public static DTOs.UserIntent ClassifyIntent(string prompt, ExtractedKeywords keywords)
        {
            var lowerPrompt = prompt.ToLowerInvariant();

            bool hasProductEntity = keywords.TranslatedKeywords
                .Any(k => ProductEntities.Contains(k)) || keywords.Sizes.Any();

            if (hasProductEntity)
                return DTOs.UserIntent.ProductSearch;

            bool hasDiscoveryTrigger = DiscoveryTriggers
                .Any(trigger => lowerPrompt.Contains(trigger));

            if (hasDiscoveryTrigger)
                return DTOs.UserIntent.Discovery;

            return DTOs.UserIntent.Chitchat;
        }
    }

    public class ExtractedKeywords
    {
        public List<string> OriginalKeywords { get; set; } = new();
        public List<string> TranslatedKeywords { get; set; } = new();
        public List<string> Sizes { get; set; } = new();

        public bool HasKeywords => OriginalKeywords.Any() || Sizes.Any();

        public List<string> GetSearchKeywords() =>
            TranslatedKeywords.Concat(Sizes).Distinct().ToList();
    }
}
