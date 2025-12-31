namespace Application.Features.Products.v1.DTOs.ProductImages
{
    public class UpsertProductImageDto
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string ImageUrl { get; set; } = default!;
        public bool IsMain { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;

        public Stream? ImgContent { get; set; }
        public string? ImgFileName { get; set; }
        public string? ImgContentType { get; set; }
    }
}
