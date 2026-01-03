using Application.Features.Products.v1.DTOs.Colors;

namespace Application.Features.Products.v1.DTOs.ProductColor
{
    public class ProductColorSummaryDto
    {
        public int Id { get; set; }
        public ColorDto Color { get; set; } = default!;
    }
}
