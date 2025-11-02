using Application.Common.Interfaces.Logging;
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.TokenRefresh;

namespace Application.Features.Auth.Commands.TokenRefresh
{
    public class TokenRefreshHandler : IRequestHandler<TokenRefreshCommand, JwtTokenResponseDto>
    {
        private readonly ITokenRefreshService _tokenRefreshService;
        private readonly ILoggingService<TokenRefreshHandler> _logger;

        public TokenRefreshHandler(
            ITokenRefreshService tokenRefreshService,
            ILoggingService<TokenRefreshHandler> logger)
        {
            _tokenRefreshService = tokenRefreshService;
            _logger = logger;
        }

        public async Task<JwtTokenResponseDto> Handle(TokenRefreshCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[TokenRefreshHandler] Attempting to refresh token: {request.refreshToken}");

            try
            {
                var result = await _tokenRefreshService.RefreshTokenAsync(request.refreshToken, cancellationToken);
                _logger.LogInformation($"[TokenRefreshHandler] Token refresh successful. New access token generated.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[TokenRefreshHandler] Token refresh failed for token: {request.refreshToken}", ex);
                throw;
            }
        }
    }
}
