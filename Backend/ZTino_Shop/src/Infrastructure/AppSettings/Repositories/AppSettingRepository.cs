using Application.Features.Appsettings.v1.Repositories;
using Domain.Models.AppSettings;
using Infrastructure.Persistence;

namespace Infrastructure.AppSettings.Repositories
{
    public class AppSettingRepository : Repository<AppSetting, Guid>, IAppSettingRepository
    {
        public AppSettingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<AppSetting?> GetByGroupAndKeyAsync(string group, string key, CancellationToken cancellationToken = default)
        {
            return await _context.AppSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Group == group && x.Key == key, cancellationToken);
        }
    }
}
