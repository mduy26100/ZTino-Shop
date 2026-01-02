namespace Application.Features.Products.v1.DTOs.ProductVariants
{
    public class ProductVariantOptionDto
    {
        public int VariantId { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; } = default!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}
