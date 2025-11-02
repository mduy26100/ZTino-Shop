using Application.Features.Auth.Services.Command.Login.Strategy;
using Domain.Enums;

namespace Application.Features.Auth.Services.Command.Login.Factory
{
    public interface ILoginStrategyFactory
    {
        ILoginStrategy GetStrategy(LoginProvider provider);
    }
}
