using Domain.Models.AppSettings;

namespace Application.Features.Appsettings.v1.Repositories
{
    public interface IAppSettingRepository : IRepository<AppSetting, Guid>
    {
        Task<AppSetting?> GetByGroupAndKeyAsync(string group, string key, CancellationToken cancellationToken = default);
    }
}
