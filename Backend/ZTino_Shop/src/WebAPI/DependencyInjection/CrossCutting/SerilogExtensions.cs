using Serilog;

namespace WebAPI.DependencyInjection.CrossCutting
{
    public static class SerilogExtensions
    {
        public static WebApplicationBuilder AddSerilogConfig(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            Log.Logger = logger;

            builder.Host.UseSerilog(logger);

            return builder;
        }
    }
}