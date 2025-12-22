using Application.Features.Products.DTOs.ProductImages;

namespace WebAPI.Models.Products.Product
{
    public class UpsertProductImageForm
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public bool IsMain { get; set; } = false;
        public List<IFormFile> ImageUrls { get; set; } = new();

        public List<UpsertProductImageDto> CreateImages()
        {
            return ImageUrls.Select(file => new UpsertProductImageDto
            {
                Id = Id,
                ProductVariantId = ProductVariantId,
                ImgContent = file.OpenReadStream(),
                ImgFileName = file.FileName,
                ImgContentType = file.ContentType
            }).ToList();
        }
    }
}
