namespace Domain.Models.Products
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    }
}
