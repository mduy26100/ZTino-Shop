namespace Domain.Models.Products
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductColorId { get; set; }
        public string ImageUrl { get; set; } = default!;
        public bool IsMain { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;

        public ProductColor ProductColor { get; set; } = default!;
    }
}
