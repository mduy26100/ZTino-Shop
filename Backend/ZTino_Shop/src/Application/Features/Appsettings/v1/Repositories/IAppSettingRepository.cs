using Domain.Models.AppSettings;

namespace Application.Features.Appsettings.v1.Repositories
{
    public interface IAppSettingRepository : IRepository<AppSetting, Guid>
    {
    }
}
