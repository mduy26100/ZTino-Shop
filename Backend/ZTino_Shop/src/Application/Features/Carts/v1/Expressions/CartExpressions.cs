using Application.Features.Carts.v1.DTOs;
using Domain.Models.Carts;
using System.Linq.Expressions;

namespace Application.Features.Carts.v1.Expressions
{
    public static class CartExpressions
    {
        public static Expression<Func<CartItem, CartItemDto>> MapToCartItemDto =>
            ci => new CartItemDto
            {
                CartItemId = ci.Id,
                ProductVariantId = ci.ProductVariantId,
                ProductId = ci.ProductVariant.ProductColor.ProductId,
                ProductName = ci.ProductVariant.ProductColor.Product.Name,
                MainImageUrl = ci.ProductVariant.ProductColor.Images
                    .Where(i => i.IsMain)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),
                SizeName = ci.ProductVariant.Size.Name,
                ColorName = ci.ProductVariant.ProductColor.Color.Name,
                Quantity = ci.Quantity,
                UnitPrice = ci.ProductVariant.Price,
                ItemTotal = ci.ProductVariant.Price * ci.Quantity,
                StockQuantity = ci.ProductVariant.StockQuantity,
                IsAvailable = ci.ProductVariant.IsActive && ci.ProductVariant.StockQuantity > 0
            };
    }
}
