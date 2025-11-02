using Application.Features.Auth.Services.Command.UpdateProfile.Strategy;

namespace Application.Features.Auth.Services.Command.UpdateProfile.Factory
{
    public interface IUpdateProfileStrategyFactory
    {
        IUpdateProfileStrategy GetSelfStrategy();
        IUpdateProfileStrategy GetManagerStrategy();
    }
}
