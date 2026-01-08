using Application.Features.Finances.v1.Repositories;
using Application.Features.Finances.v1.Services;
using Infrastructure.Finances.Repositories;
using Infrastructure.Finances.Services;

namespace WebAPI.DependencyInjection.Features
{
    public static class FinancesRegistration
    {
        public static IServiceCollection AddFinancesFeature(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();

            // Services
            services.AddScoped<IInvoiceService, InvoiceService>();

            return services;
        }
    }
}
