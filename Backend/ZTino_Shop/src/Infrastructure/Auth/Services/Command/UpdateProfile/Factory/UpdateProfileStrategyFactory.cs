using Application.Features.Auth.v1.Services.Command.UpdateProfile.Factory;
using Application.Features.Auth.v1.Services.Command.UpdateProfile.Strategy;
using Infrastructure.Auth.Services.Command.UpdateProfile.Strategies;

namespace Infrastructure.Auth.Services.Command.UpdateProfile.Factory
{
    public class UpdateProfileStrategyFactory : IUpdateProfileStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateProfileStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUpdateProfileStrategy GetSelfStrategy()
            => _serviceProvider.GetRequiredService<SelfUpdateProfileStrategy>();

        public IUpdateProfileStrategy GetManagerStrategy()
            => _serviceProvider.GetRequiredService<ManagerUpdateProfileStrategy>();
    }
}
