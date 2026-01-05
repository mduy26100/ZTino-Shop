using Application.Common.Abstractions.ExternalServices;
using Infrastructure.ExternalServices.Cloudinary;

namespace WebAPI.DependencyInjection.Infrastructure
{
    public static class ExternalServicesRegistration
    {
        public static IServiceCollection AddExternalServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Cloudinary file upload
            services.Configure<CloudinarySettings>(
                configuration.GetSection("Cloudinary"));

            services.AddScoped<IFileUploadService, CloudinaryFileUploadService>();

            return services;
        }
    }
}
