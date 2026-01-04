namespace Domain.Models.Orders
{
    public class OrderAddress
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }

        public string RecipientName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public string Street { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public string District { get; set; } = default!;
        public string City { get; set; } = default!;

        public string FullAddress => $"{Street}, {Ward}, {District}, {City}";

        public Order Order { get; set; } = default!;
    }
}