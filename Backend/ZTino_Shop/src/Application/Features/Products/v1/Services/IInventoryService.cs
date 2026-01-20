using Domain.Models.Carts;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Services
{
    public interface IInventoryService
    {
        Task<IReadOnlyList<(ProductVariant Variant, int Quantity)>> PrepareAndValidateStockAsync(
            IEnumerable<CartItem> cartItems,
            CancellationToken cancellationToken = default);
    }
}
