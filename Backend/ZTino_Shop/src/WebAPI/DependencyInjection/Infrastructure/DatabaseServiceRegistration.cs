using Application.Common.Interfaces.Persistence.Data;
using Infrastructure.Data;

namespace WebAPI.DependencyInjection.Infrastructure
{
    public static class DatabaseServiceRegistration
    {
        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            return services;
        }
    }
}
