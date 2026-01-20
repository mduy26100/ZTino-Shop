using Application.Common.Exceptions;
using Application.Features.Products.v1.Repositories;
using Application.Features.Products.v1.Services;
using Domain.Models.Carts;
using Domain.Models.Products;

namespace Infrastructure.Products.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IProductVariantRepository _variantRepository;

        public InventoryService(IProductVariantRepository variantRepository)
        {
            _variantRepository = variantRepository;
        }

        public async Task<IReadOnlyList<(ProductVariant, int)>> PrepareAndValidateStockAsync(
            IEnumerable<CartItem> cartItems,
            CancellationToken ct)
        {
            var variantIds = cartItems.Select(ci => ci.ProductVariantId).Distinct().ToList();
            var variants = await _variantRepository.GetManyWithDetailsForOrderAsync(variantIds, ct);

            var result = new List<(ProductVariant, int)>();
            foreach (var item in cartItems)
            {
                var variant = variants.FirstOrDefault(v => v.Id == item.ProductVariantId)
                    ?? throw new NotFoundException($"Product variant {item.ProductVariantId} not found.");

                variant.EnsureActive();
                if (!variant.CanFulfill(item.Quantity))
                {
                    throw new BusinessRuleException(
                        $"Insufficient stock for variant {variant.Id}. " +
                        $"Requested: {item.Quantity}, Available: {variant.StockQuantity}.");
                }

                result.Add((variant, item.Quantity));
            }

            return result;
        }
    }
}
