namespace Application.Features.Auth.Services.Command.Logout
{
    public interface ILogoutService
    {
        Task<bool> LogoutAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
