using Application.Features.Stats.v1.Repositories;
using Domain.Models.Stats;
using Infrastructure.Persistence;

namespace Infrastructure.Stats.Repositories
{
    public class ProductSalesStatsRepository : Repository<ProductSalesStats, int>, IProductSalesStatsRepository
    {
        public ProductSalesStatsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
