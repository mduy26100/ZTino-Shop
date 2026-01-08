using Application.Features.Products.v1.Repositories;
using Application.Features.Products.v1.Services;
using Domain.Models.Orders;

namespace Infrastructure.Products.Services
{
    public class StockService : IStockService
    {
        private readonly IProductVariantRepository _productVariantRepository;

        public StockService(IProductVariantRepository productVariantRepository)
        {
            _productVariantRepository = productVariantRepository;
        }

        public async Task RestoreStockAsync(ICollection<OrderItem> orderItems, CancellationToken cancellationToken = default)
        {
            foreach (var item in orderItems)
            {
                var variant = await _productVariantRepository.GetByIdAsync(item.ProductVariantId, cancellationToken);

                if (variant != null)
                {
                    variant.StockQuantity += item.Quantity;
                    _productVariantRepository.Update(variant);
                }
            }
        }
    }
}
