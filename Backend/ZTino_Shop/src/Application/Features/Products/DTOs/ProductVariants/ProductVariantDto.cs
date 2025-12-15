using Application.Features.Products.DTOs.Colors;
using Application.Features.Products.DTOs.ProductImages;
using Application.Features.Products.DTOs.Sizes;

namespace Application.Features.Products.DTOs.ProductVariants
{
    public class ProductVariantDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ColorDto Color { get; set; } = default!;
        public SizeDto Size { get; set; } = default!;
        public List<ProductImageDto> Images { get; set; } = new();
    }
}
