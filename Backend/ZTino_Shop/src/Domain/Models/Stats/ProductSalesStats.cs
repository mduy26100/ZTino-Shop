namespace Domain.Models.Stats
{
    public class ProductSalesStats
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; } = default!;
        public string Sku { get; set; } = default!;

        public int TotalSoldQuantity { get; set; }
        public decimal TotalRevenue { get; set; }

        public DateTime LastSoldAt { get; set; }
    }
}