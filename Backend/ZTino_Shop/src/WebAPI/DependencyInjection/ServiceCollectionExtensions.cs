using WebAPI.DependencyInjection.Application;
using WebAPI.DependencyInjection.CrossCutting;
using WebAPI.DependencyInjection.Features;
using WebAPI.DependencyInjection.Infrastructure;
using WebAPI.DependencyInjection.Security;

namespace WebAPI.DependencyInjection
{
    /// <summary>
    /// Main entry point for all service registrations.
    /// Groups services by architectural layer for clarity.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all application services in the correct order.
        /// </summary>
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddInfrastructure(configuration)
                .AddApplicationCore()
                .AddSecurity(configuration)
                .AddFeatures()
                .AddCrossCutting();
        }

        /// <summary>
        /// Infrastructure layer: Database, Cache, External Services
        /// </summary>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            services.AddCaching(configuration);
            services.AddExternalServices(configuration);
            services.AddInfrastructureSecurity();
            return services;
        }

        /// <summary>
        /// Application layer: MediatR, Mapping, Behaviors
        /// </summary>
        public static IServiceCollection AddApplicationCore(this IServiceCollection services)
        {
            services.AddMediatRPipeline();
            services.AddMapping();
            return services;
        }

        /// <summary>
        /// Security layer: Identity, JWT Authentication
        /// </summary>
        public static IServiceCollection AddSecurity(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore(configuration);
            services.AddJwtAuthentication(configuration);
            return services;
        }

        /// <summary>
        /// Feature modules: Auth, Products, Carts, etc.
        /// </summary>
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            services.AddAuthFeature();
            services.AddProductsFeature();
            services.AddCartsFeature();
            services.AddOrdersFeature();
            services.AddFinancesFeature();
            services.AddStatsFeature();
            return services;
        }

        /// <summary>
        /// Cross-cutting concerns: CORS, API Versioning, etc.
        /// </summary>
        public static IServiceCollection AddCrossCutting(this IServiceCollection services)
        {
            services.AddCorsPolicies();
            services.AddApiVersioningConfig();
            return services;
        }
    }
}