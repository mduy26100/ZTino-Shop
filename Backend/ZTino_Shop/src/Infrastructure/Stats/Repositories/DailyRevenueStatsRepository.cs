using Application.Features.Stats.v1.Repositories;
using Domain.Models.Stats;
using Infrastructure.Persistence;

namespace Infrastructure.Stats.Repositories
{
    public class DailyRevenueStatsRepository : Repository<DailyRevenueStats, int>, IDailyRevenueStatsRepository
    {
        public DailyRevenueStatsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
