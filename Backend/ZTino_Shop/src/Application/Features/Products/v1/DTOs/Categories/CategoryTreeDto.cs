namespace Application.Features.Products.v1.DTOs.Categories
{
    public class CategoryTreeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public int? ParentId { get; set; }
        public List<CategoryTreeDto> Children { get; set; } = new();
    }
}
