using Domain.Models.Finances;

namespace Application.Features.Finances.v1.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice, Guid>
    {
    }
}
