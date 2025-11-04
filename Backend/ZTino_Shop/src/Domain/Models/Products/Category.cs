namespace Domain.Models.Products
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public int? ParentId { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public Category? Parent { get; set; }

        public ICollection<Category> Children { get; set; } = new List<Category>();
    }
}
