namespace Application.Features.Products.DTOs.Products
{
    public class UpsertProductDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }
        public string MainImageUrl { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public DateTime? UpdatedAt { get; set; }

        public Stream? ImgContent { get; set; }
        public string? ImgFileName { get; set; }
        public string? ImgContentType { get; set; }
    }
}
