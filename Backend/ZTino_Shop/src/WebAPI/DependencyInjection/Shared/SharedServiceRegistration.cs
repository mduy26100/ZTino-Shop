using Application.Common.Behaviors;
using Application.Common.Interfaces.Logging;
using Application.Features.Auth.Commands.Login;
using Infrastructure.Common.Interfaces.Logging;

namespace WebAPI.DependencyInjection.Shared
{
    public static class SharedServiceRegistration
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {

            //Add MediatR
            services.AddMediatR(typeof(LoginCommand).Assembly);

            //Logging
            services.AddScoped(typeof(ILoggingService<>), typeof(LoggingService<>));

            //Add FluentValidation
            services.AddValidatorsFromAssemblyContaining<LoginValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
