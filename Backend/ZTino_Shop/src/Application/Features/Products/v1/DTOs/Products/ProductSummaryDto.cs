namespace Application.Features.Products.v1.DTOs.Products
{
    public class ProductSummaryDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public decimal BasePrice { get; set; }
        public string MainImageUrl { get; set; } = default!;
        public bool IsActive { get; set; }

        public string? CategoryName { get; set; }
    }
}
