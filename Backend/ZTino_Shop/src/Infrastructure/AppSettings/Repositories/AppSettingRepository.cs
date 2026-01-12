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
    }
}
