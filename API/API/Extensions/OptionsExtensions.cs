using API.Options;

namespace API.Extensions
{
    public static class OptionsExtensions
    {
        public static IServiceCollection AddOptionsConfiguration(
     this IServiceCollection services,
     IConfiguration configuration)
        {
            services.Configure<DBConnectionOptions>(
                configuration.GetSection("ConnectionStrings"));

            services.Configure<AuditTrailOptions>(
              configuration.GetSection("AuditLogging"));

            return services;
        }
    }
}
