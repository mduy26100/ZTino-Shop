using Application.Features.Products.v1.DTOs.Categories;
using Application.Features.Products.v1.DTOs.ProductVariants;

namespace Application.Features.Products.v1.DTOs.Products
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }
        public string MainImageUrl { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CategoryDto Category { get; set; } = default!;
        public List<ProductVariantDto> Variants { get; set; } = new();
        public List<ProductVariantGroupDto> VariantGroups { get; set; } = new();
    }
}
