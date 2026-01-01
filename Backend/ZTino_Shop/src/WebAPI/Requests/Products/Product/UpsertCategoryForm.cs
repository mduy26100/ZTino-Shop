using Application.Features.Products.v1.DTOs.Categories;

namespace WebAPI.Requests.Products.Product
{
    public class UpsertCategoryForm
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public int? ParentId { get; set; }
        public IFormFile? ImageUrl { get; set; }

        public UpsertCategoryDto ToDto()
        {
            return new UpsertCategoryDto
            {
                Id = Id,
                Name = Name,
                Slug = Slug,
                IsActive = IsActive,
                ImgContent = ImageUrl?.OpenReadStream(),
                ImgFileName = ImageUrl?.FileName,
                ImgContentType = ImageUrl?.ContentType,
            };
        }
    }
}
