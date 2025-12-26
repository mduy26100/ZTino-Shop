using Application.Common.Interfaces.Identity;
using Application.Common.Interfaces.Identity;
using Application.Features.Auth.Services.Command.Login;
using Application.Features.Auth.Services.Command.Login.Factory;
using Application.Features.Auth.Services.Command.Login.Strategy;
using Application.Features.Auth.Services.Command.Logout;
using Application.Features.Auth.Services.Command.Register;
using Application.Features.Auth.Services.Command.TokenRefresh;
using Application.Features.Auth.Services.Command.UpdateProfile;
using Application.Features.Auth.Services.Command.UpdateProfile.Factory;
using Application.Features.Auth.Services.Jwt;
using Application.Features.Auth.Services.Query.CurrentUser;
using Infrastructure.Auth.Models;
using Infrastructure.Auth.Options;
using Infrastructure.Auth.Services.Command.Login;
using Infrastructure.Auth.Services.Command.Login.Strategies;
using Infrastructure.Auth.Services.Command.Logout;
using Infrastructure.Auth.Services.Command.Register;
using Infrastructure.Auth.Services.Command.TokenRefresh;
using Infrastructure.Auth.Services.Command.UpdateProfile;
using Infrastructure.Auth.Services.Command.UpdateProfile.Factory;
using Infrastructure.Auth.Services.Command.UpdateProfile.Strategies;
using Infrastructure.Auth.Services.Jwt;
using Infrastructure.Auth.Services.Query.CurrentUser;
using Infrastructure.Common.Interfaces.Identity;
using Infrastructure.Data;

namespace WebAPI.DependencyInjection.Auth
{
    public static class AuthServiceRegistration
    {
        public static IServiceCollection AddAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            // ===== Identity =====
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // ===== JWT =====
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;

                    var jwtSecret = configuration["JWT:Secret"]
                        ?? throw new InvalidOperationException("JWT Secret is not configured");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        ValidAudience = configuration["JWT:ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // ===== Core Services =====
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ICurrentUser, CurrentUser>();

            // ===== Login =====
            services.AddScoped<ILoginStrategyFactory, LoginStrategyFactory>();
            services.AddScoped<ILoginStrategy, EmailPasswordLoginStrategy>();
            services.AddScoped<ILoginStrategy, FacebookLoginStrategy>();
            services.AddScoped<ILoginStrategy, GoogleLoginStrategy>();
            services.AddScoped<ILoginService, LoginService>();


            // ===== Register =====
            services.AddScoped<IRegisterService, RegisterService>();


            //===== Token Refresh =====
            services.AddScoped<ITokenRefreshService, TokenRefreshService>();

            //===== Current User =====
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // ===== Update Profile =====
            services.AddScoped<IUpdateProfileStrategyFactory, UpdateProfileStrategyFactory>();
            services.AddScoped<SelfUpdateProfileStrategy>();
            services.AddScoped<ManagerUpdateProfileStrategy>();
            services.AddScoped<IUpdateProfileService, UpdateProfileService>();

            //===== Logout =====
            services.AddScoped<ILogoutService, LogoutService>();

            return services;
        }
    }
}
