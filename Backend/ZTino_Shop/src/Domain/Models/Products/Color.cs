namespace Domain.Models.Products
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
    }
}
