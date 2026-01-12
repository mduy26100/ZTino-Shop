using Application.Features.AI.v1.DTOs;
using Application.Features.AI.v1.Services;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Infrastructure.ExternalServices.AI
{
    public class AIContextService : IAIContextService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private const int MaxFlattenedResults = 15;

        public AIContextService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<AIProductContextDto>> GetProductContextAsync(
            ExtractedKeywords keywords,
            CancellationToken cancellationToken = default)
        {
            if (!keywords.HasKeywords)
                return new List<AIProductContextDto>();

            var searchTerms = keywords.GetSearchKeywords();

            var predicate = BuildAndSearchPredicate(searchTerms);

            var products = await _productRepository
                .GetAll(predicate)
                .Include(p => p.Category)
                .Include(p => p.ProductColors)
                    .ThenInclude(pc => pc.Color)
                .Include(p => p.ProductColors)
                    .ThenInclude(pc => pc.ProductVariants)
                        .ThenInclude(pv => pv.Size)
                .Where(p => p.IsActive)
                .Take(20)
                .ToListAsync(cancellationToken);

            var flattened = FlattenAndGroupProducts(products, keywords.Sizes);

            return flattened.Take(MaxFlattenedResults).ToList();
        }

        public async Task<AIDiscoveryContextDto> GetDiscoveryContextAsync(
            CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepository
                .GetAll(c => c.IsActive)
                .Select(c => c.Name)
                .ToListAsync(cancellationToken);

            var featuredProducts = await _productRepository
                .GetAll(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.ProductColors)
                    .ThenInclude(pc => pc.Color)
                .Include(p => p.ProductColors)
                    .ThenInclude(pc => pc.ProductVariants)
                        .ThenInclude(pv => pv.Size)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .ToListAsync(cancellationToken);

            var flattenedProducts = FlattenAndGroupProducts(featuredProducts, new List<string>());

            return new AIDiscoveryContextDto
            {
                Categories = categories,
                FeaturedProducts = flattenedProducts.Take(5).ToList()
            };
        }

        private static Expression<Func<Product, bool>> BuildAndSearchPredicate(List<string> keywords)
        {
            var parameter = Expression.Parameter(typeof(Product), "p");
            Expression? combined = null;

            foreach (var keyword in keywords)
            {
                var keywordLower = keyword.ToLowerInvariant();

                var nameContains = BuildContainsExpression(parameter, "Name", keywordLower);
                var descContains = BuildContainsExpression(parameter, "Description", keywordLower);

                var keywordMatch = Expression.OrElse(nameContains, descContains);

                combined = combined is null
                    ? keywordMatch
                    : Expression.AndAlso(combined, keywordMatch);
            }

            return Expression.Lambda<Func<Product, bool>>(
                combined ?? Expression.Constant(true),
                parameter);
        }

        private static Expression BuildContainsExpression(
            ParameterExpression parameter,
            string propertyName,
            string keyword)
        {
            var property = Expression.Property(parameter, propertyName);

            var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));

            var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;

            var toLowerCall = Expression.Call(property, toLowerMethod);
            var containsCall = Expression.Call(toLowerCall, containsMethod, Expression.Constant(keyword));

            return Expression.AndAlso(nullCheck, containsCall);
        }

        private static List<AIProductContextDto> FlattenAndGroupProducts(
            List<Product> products,
            List<string> requestedSizes)
        {
            var result = new List<AIProductContextDto>();

            foreach (var product in products)
            {
                foreach (var productColor in product.ProductColors)
                {
                    var variants = productColor.ProductVariants
                        .Where(v => v.IsActive)
                        .ToList();

                    if (!variants.Any())
                        continue;

                    if (requestedSizes.Any())
                    {
                        variants = variants
                            .Where(v => requestedSizes.Contains(v.Size?.Name ?? "", StringComparer.OrdinalIgnoreCase))
                            .ToList();

                        if (!variants.Any())
                            continue;
                    }

                    var availableSizes = string.Join(", ",
                        variants
                            .Where(v => v.StockQuantity > 0)
                            .Select(v => v.Size?.Name ?? "")
                            .Distinct()
                            .OrderBy(s => GetSizeOrder(s)));

                    var totalStock = variants.Sum(v => v.StockQuantity);
                    var stockStatus = StockStatusHelper.FromQuantity(totalStock);

                    var prices = variants.Select(v => v.Price).ToList();

                    result.Add(new AIProductContextDto
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        CategoryName = product.Category?.Name ?? "Unknown",
                        BasePrice = product.BasePrice,
                        Description = TruncateDescription(product.Description, 100),
                        ColorName = productColor.Color?.Name ?? "Unknown",
                        AvailableSizes = string.IsNullOrEmpty(availableSizes)
                            ? "None"
                            : availableSizes,
                        StockStatus = stockStatus,
                        MinPrice = prices.Min(),
                        MaxPrice = prices.Max()
                    });
                }
            }

            return result;
        }

        private static int GetSizeOrder(string? size)
        {
            if (string.IsNullOrEmpty(size))
                return 999;

            if (int.TryParse(size, out var numericSize))
                return numericSize;

            return size.ToUpperInvariant() switch
            {
                "XS" => 0,
                "S" => 1,
                "M" => 2,
                "L" => 3,
                "XL" => 4,
                "XXL" or "2XL" => 5,
                "XXXL" or "3XL" => 6,
                "4XL" => 7,
                "5XL" => 8,
                _ => 100
            };
        }

        private static string? TruncateDescription(string? desc, int maxLength)
        {
            if (string.IsNullOrEmpty(desc)) return null;
            return desc.Length <= maxLength ? desc : desc[..maxLength] + "...";
        }
    }
}
