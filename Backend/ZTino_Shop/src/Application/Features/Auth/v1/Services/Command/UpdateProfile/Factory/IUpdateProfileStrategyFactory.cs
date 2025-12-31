using Application.Features.Auth.v1.Services.Command.UpdateProfile.Strategy;

namespace Application.Features.Auth.v1.Services.Command.UpdateProfile.Factory
{
    public interface IUpdateProfileStrategyFactory
    {
        IUpdateProfileStrategy GetSelfStrategy();
        IUpdateProfileStrategy GetManagerStrategy();
    }
}
