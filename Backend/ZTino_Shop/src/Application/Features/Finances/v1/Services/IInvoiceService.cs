using Domain.Models.Orders;

namespace Application.Features.Finances.v1.Services
{
    public interface IInvoiceService
    {
        Task UpsertInvoiceAsync(Order order, CancellationToken cancellationToken = default);
        string GenerateInvoiceNumber();
    }
}
