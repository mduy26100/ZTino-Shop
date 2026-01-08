using Application.Features.Finances.v1.Repositories;
using Application.Features.Finances.v1.Services;
using Domain.Constants;
using Domain.Models.Finances;
using Domain.Models.Orders;

namespace Infrastructure.Finances.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task UpsertInvoiceAsync(Order order, CancellationToken cancellationToken = default)
        {
            if (order.Invoice != null)
            {
                order.Invoice.Status = InvoiceStatus.Paid;
            }
            else
            {
                var invoice = new Invoice
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    InvoiceNumber = GenerateInvoiceNumber(),
                    IssuedDate = DateTime.UtcNow,
                    CustomerName = order.CustomerName,
                    TotalAmount = order.TotalAmount,
                    TaxAmount = order.TaxAmount,
                    Status = InvoiceStatus.Paid
                };

                await _invoiceRepository.AddAsync(invoice, cancellationToken);

                order.InvoiceId = invoice.Id;
            }
        }

        public string GenerateInvoiceNumber()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var randomPart = Guid.NewGuid().ToString("N")[..4].ToUpperInvariant();
            return $"INV-{timestamp}-{randomPart}";
        }
    }
}
