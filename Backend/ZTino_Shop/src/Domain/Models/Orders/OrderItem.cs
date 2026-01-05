using Domain.Models.Products;

namespace Domain.Models.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }

        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; } = default!;
        public string Sku { get; set; } = default!;

        public string ColorName { get; set; } = default!;
        public string SizeName { get; set; } = default!;
        public string? ThumbnailUrl { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalLineAmount { get; set; }

        public Order Order { get; set; } = default!;

        public static OrderItem CreateFromVariant(ProductVariant variant, int quantity)
        {
            var productColor = variant.ProductColor;
            var product = productColor?.Product;
            var color = productColor?.Color;
            var size = variant.Size;
            var category = product?.Category;

            var thumbnailUrl = productColor?.Images?.FirstOrDefault()?.ImageUrl;

            return new OrderItem
            {
                ProductVariantId = variant.Id,
                ProductId = product?.Id ?? 0,
                ProductName = product?.Name ?? "Unknown Product",
                Sku = $"{product?.Slug ?? "unknown"}-{color?.Name ?? "unknown"}-{size?.Name ?? "unknown"}",
                ColorName = color?.Name ?? "Unknown",
                SizeName = size?.Name ?? "Unknown",
                ThumbnailUrl = thumbnailUrl ?? product?.MainImageUrl,
                CategoryId = category?.Id ?? 0,
                CategoryName = category?.Name ?? "Unknown",
                Quantity = quantity,
                UnitPrice = variant.Price,
                TotalLineAmount = variant.Price * quantity
            };
        }
    }
}