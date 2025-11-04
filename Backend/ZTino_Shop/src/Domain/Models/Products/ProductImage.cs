namespace Domain.Models.Products
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string ImageUrl { get; set; } = default!;
        public bool IsMain { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;

        public ProductVariant ProductVariant { get; set; } = default!;
    }
}
