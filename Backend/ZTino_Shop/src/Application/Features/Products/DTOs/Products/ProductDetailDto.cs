using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.DTOs.ProductVariants;

namespace Application.Features.Products.DTOs.Products
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }
        public string MainImageUrl { get; set; } = default!;
        public CategoryDto Category { get; set; } = default!;
        public List<ProductVariantDto> Variants { get; set; } = new();
    }
}
