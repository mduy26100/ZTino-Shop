using Application.Common.Behaviors;
using Application.Features.Auth.v1.Commands.Login;

namespace WebAPI.DependencyInjection.Shared
{
    public static class SharedServiceRegistration
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {

            //Add MediatR
            services.AddMediatR(typeof(LoginCommand).Assembly);

            //Add FluentValidation
            services.AddValidatorsFromAssemblyContaining<LoginValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
