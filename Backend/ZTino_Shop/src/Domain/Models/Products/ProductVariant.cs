namespace Domain.Models.Products
{
    public class ProductVariant
    {
        public int Id { get; set; }

        public int ProductColorId { get; set; }
        public int SizeId { get; set; }

        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

        public ProductColor ProductColor { get; set; } = default!;
        public Size Size { get; set; } = default!;
    }
}
