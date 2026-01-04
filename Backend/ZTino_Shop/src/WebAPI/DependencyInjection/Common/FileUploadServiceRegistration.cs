using Application.Common.Abstractions.ExternalServices;
using Infrastructure.ExternalServices.Cloudinary;

namespace WebAPI.DependencyInjection.Common
{
    public static class FileUploadServiceRegistration
    {
        public static IServiceCollection AddFileUploadServices(this IServiceCollection services
            , IConfiguration configuration)
        {
            // Bind Cloudinary settings
            services.Configure<CloudinarySettings>(
                configuration.GetSection("Cloudinary"));

            // FileUpload
            services.AddScoped<IFileUploadService, CloudinaryFileUploadService>();

            return services;
        }
    }
}
