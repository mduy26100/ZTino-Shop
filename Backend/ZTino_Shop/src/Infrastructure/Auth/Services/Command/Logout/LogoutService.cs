using Application.Features.Auth.Services.Command.Logout;
using Infrastructure.Data;

namespace Infrastructure.Auth.Services.Command.Logout
{
    public class LogoutService : ILogoutService
    {
        private readonly ApplicationDbContext _dbContext;

        public LogoutService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var tokens = await _dbContext.RefreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            if (!tokens.Any())
                return false;

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
