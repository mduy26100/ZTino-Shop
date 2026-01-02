using Application.Features.Products.v1.DTOs.Sizes;

namespace Application.Features.Products.v1.DTOs.ProductVariants
{
    public class ProductVariantDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public SizeDto Size { get; set; } = default!;
    }
}
