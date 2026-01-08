using Domain.Models.Orders;

namespace Application.Features.Finances.v1.Services
{
    /// <summary>
    /// Service for invoice operations.
    /// </summary>
    public interface IInvoiceService
    {
        /// <summary>
        /// Creates a new invoice or updates existing one for the order.
        /// </summary>
        Task UpsertInvoiceAsync(Order order, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates a unique invoice number.
        /// </summary>
        string GenerateInvoiceNumber();
    }
}
