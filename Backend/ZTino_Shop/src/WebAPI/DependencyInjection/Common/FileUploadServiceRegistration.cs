using Application.Common.Configurations;
using Application.Common.Interfaces.Services.FileUpLoad;
using Infrastructure.Common.Interfaces.Services.FileUpLoad;

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

            //FileUpLoad
            services.AddScoped<IFileUploadService, FileUploadService>();

            return services;
        }
    }
}
