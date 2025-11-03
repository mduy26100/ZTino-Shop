namespace Domain.Models.Products
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public int? ParentId { get; set; }
    }
}
