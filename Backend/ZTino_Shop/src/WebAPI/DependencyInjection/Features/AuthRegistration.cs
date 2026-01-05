using Application.Features.Auth.v1.Services.Command.Login;
using Application.Features.Auth.v1.Services.Command.Login.Factory;
using Application.Features.Auth.v1.Services.Command.Login.Strategy;
using Application.Features.Auth.v1.Services.Command.Logout;
using Application.Features.Auth.v1.Services.Command.Register;
using Application.Features.Auth.v1.Services.Command.TokenRefresh;
using Application.Features.Auth.v1.Services.Command.UpdateProfile;
using Application.Features.Auth.v1.Services.Command.UpdateProfile.Factory;
using Application.Features.Auth.v1.Services.Query.CurrentUser;
using Infrastructure.Auth.Services.Command.Login;
using Infrastructure.Auth.Services.Command.Login.Strategies;
using Infrastructure.Auth.Services.Command.Logout;
using Infrastructure.Auth.Services.Command.Register;
using Infrastructure.Auth.Services.Command.TokenRefresh;
using Infrastructure.Auth.Services.Command.UpdateProfile;
using Infrastructure.Auth.Services.Command.UpdateProfile.Factory;
using Infrastructure.Auth.Services.Command.UpdateProfile.Strategies;
using Infrastructure.Auth.Services.Query.CurrentUser;

namespace WebAPI.DependencyInjection.Features
{
    public static class AuthRegistration
    {
        public static IServiceCollection AddAuthFeature(this IServiceCollection services)
        {
            // ===== Login =====
            services.AddScoped<ILoginStrategyFactory, LoginStrategyFactory>();
            services.AddScoped<ILoginStrategy, EmailPasswordLoginStrategy>();
            services.AddScoped<ILoginStrategy, FacebookLoginStrategy>();
            services.AddScoped<ILoginStrategy, GoogleLoginStrategy>();
            services.AddScoped<ILoginService, LoginService>();

            // ===== Register =====
            services.AddScoped<IRegisterService, RegisterService>();

            // ===== Token Refresh =====
            services.AddScoped<ITokenRefreshService, TokenRefreshService>();

            // ===== Current User Query =====
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // ===== Update Profile =====
            services.AddScoped<IUpdateProfileStrategyFactory, UpdateProfileStrategyFactory>();
            services.AddScoped<SelfUpdateProfileStrategy>();
            services.AddScoped<ManagerUpdateProfileStrategy>();
            services.AddScoped<IUpdateProfileService, UpdateProfileService>();

            // ===== Logout =====
            services.AddScoped<ILogoutService, LogoutService>();

            return services;
        }
    }
}