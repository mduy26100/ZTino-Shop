namespace Application.Features.Auth.v1.Commands.TokenRefresh
{
    public class TokenRefreshValidator : AbstractValidator<TokenRefreshCommand>
    {
        public TokenRefreshValidator()
        {
            RuleFor(x => x.refreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
