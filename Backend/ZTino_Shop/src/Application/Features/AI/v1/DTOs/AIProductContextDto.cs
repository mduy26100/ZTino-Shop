namespace Application.Features.AI.v1.DTOs
{
    public class AIProductContextDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }

        public string ColorName { get; set; } = default!;

        public string AvailableSizes { get; set; } = default!;

        public string StockStatus { get; set; } = default!;

        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }

    public static class StockStatusHelper
    {
        public const string InStock = "In Stock";
        public const string LowStock = "Low Stock";
        public const string OutOfStock = "Out of Stock";

        public static string FromQuantity(int quantity) => quantity switch
        {
            > 10 => InStock,
            > 0 => LowStock,
            _ => OutOfStock
        };
    }
}
