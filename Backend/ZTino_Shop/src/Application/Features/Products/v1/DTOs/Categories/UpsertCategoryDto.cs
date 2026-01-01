namespace Application.Features.Products.v1.DTOs.Categories
{
    public class UpsertCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public int? ParentId { get; set; }

        public Stream? ImgContent { get; set; }
        public string? ImgFileName { get; set; }
        public string? ImgContentType { get; set; }
    }
}
