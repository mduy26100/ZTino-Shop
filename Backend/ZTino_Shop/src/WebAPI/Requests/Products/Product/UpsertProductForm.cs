using Application.Features.Products.v1.DTOs.Products;

namespace WebAPI.Requests.Products.Product
{
    public class UpsertProductForm
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? UpdatedAt { get; set; }
        public IFormFile? MainImageUrl { get; set; }

        public UpsertProductDto ToDto()
        {
            return new UpsertProductDto
            {
                Id = Id,
                CategoryId = CategoryId,
                Name = Name,
                Slug = Slug,
                BasePrice = BasePrice,
                Description = Description,
                IsActive = IsActive,
                UpdatedAt = UpdatedAt,
                ImgContent = MainImageUrl?.OpenReadStream(),
                ImgFileName = MainImageUrl?.FileName,
                ImgContentType = MainImageUrl?.ContentType,
            };
        }
    }
}
