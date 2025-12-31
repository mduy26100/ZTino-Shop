using Application.Features.Auth.v1.Services.Command.Login.Strategy;
using Domain.Enums;

namespace Application.Features.Auth.v1.Services.Command.Login.Factory
{
    public interface ILoginStrategyFactory
    {
        ILoginStrategy GetStrategy(LoginProvider provider);
    }
}
