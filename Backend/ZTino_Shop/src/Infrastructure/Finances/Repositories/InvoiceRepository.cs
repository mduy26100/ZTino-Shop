using Application.Features.Finances.v1.Repositories;
using Domain.Models.Finances;
using Infrastructure.Persistence;

namespace Infrastructure.Finances.Repositories
{
    public class InvoiceRepository : Repository<Invoice, Guid>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
