using Application.Features.Products.DTOs.ProductImages;

namespace WebAPI.Requests.Products.Product
{
    public class UpsertProductImageForm
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public bool IsMain { get; set; } = false;
        public List<IFormFile> ImageFiles { get; set; } = new();
        public IFormFile? ImageFile { get; set; }

        public List<UpsertProductImageDto> CreateImages()
        {
            return ImageFiles.Select(file => new UpsertProductImageDto
            {
                Id = Id,
                ProductVariantId = ProductVariantId,
                ImgContent = file.OpenReadStream(),
                ImgFileName = file.FileName,
                ImgContentType = file.ContentType
            }).ToList();
        }

        public UpsertProductImageDto UpdateImage()
        {
            return new UpsertProductImageDto
            {
                Id = Id,
                IsMain = IsMain,
                ProductVariantId = ProductVariantId,
                ImgContent = ImageFile?.OpenReadStream(),
                ImgFileName = ImageFile?.FileName,
                ImgContentType = ImageFile?.ContentType
            };
        }
    }
}
