using Application.Common.Behaviors;
using Application.Features.Auth.v1.Commands.Login;

namespace WebAPI.DependencyInjection.Application
{
    public static class MediatRRegistration
    {
        public static IServiceCollection AddMediatRPipeline(this IServiceCollection services)
        {
            // MediatR - scan Application assembly for handlers
            services.AddMediatR(typeof(LoginCommand).Assembly);

            // FluentValidation - scan for validators
            services.AddValidatorsFromAssemblyContaining<LoginValidator>();

            // Pipeline behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}