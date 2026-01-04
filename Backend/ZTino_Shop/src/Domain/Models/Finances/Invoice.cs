using Domain.Constants;
using Domain.Models.Orders;

namespace Domain.Models.Finances
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }

        public string InvoiceNumber { get; set; } = default!;
        public DateTime IssuedDate { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

        public string CustomerName { get; set; } = default!;
        public string? TaxCode { get; set; }
        public string? CompanyAddress { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }

        public string? InvoicePdfUrl { get; set; }
        public string Status { get; set; } = InvoiceStatus.Unpaid;

        public Order Order { get; set; } = default!;
    }
}