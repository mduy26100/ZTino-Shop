using Application.Features.Auth.v1.DTOs;
using Application.Features.Auth.v1.Services.Command.TokenRefresh;

namespace Application.Features.Auth.v1.Commands.TokenRefresh
{
    public class TokenRefreshHandler : IRequestHandler<TokenRefreshCommand, JwtTokenResponseDto>
    {
        private readonly ITokenRefreshService _tokenRefreshService;

        public TokenRefreshHandler(
            ITokenRefreshService tokenRefreshService)
        {
            _tokenRefreshService = tokenRefreshService;
        }

        public async Task<JwtTokenResponseDto> Handle(TokenRefreshCommand request, CancellationToken cancellationToken)
        {
            var result = await _tokenRefreshService.RefreshTokenAsync(request.refreshToken, cancellationToken);
            return result;
        }
    }
}
