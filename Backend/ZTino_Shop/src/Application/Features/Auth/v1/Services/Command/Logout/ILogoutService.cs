namespace Application.Features.Auth.v1.Services.Command.Logout
{
    public interface ILogoutService
    {
        Task<bool> LogoutAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
