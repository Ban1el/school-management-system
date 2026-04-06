using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Extensions;
using API.Services;
using API.Repositories.Interfaces;
using API.Repositories;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddScoped<UserAuthService>();
        services.AddScoped<TokenService>();
        services.AddScoped<ErrorLogService>();
        services.AddScoped<AuditTrailService>();
        services.AddScoped<AddressService>();
        services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(config.GetConnectionString("SMSDatabase")));
        string base64Key = config["Encryption:Key"]
             ?? throw new InvalidOperationException("Encryption key not found in configuration.");
        CryptoExtensions.SetKey(base64Key);
        services.AddIdentityServices(config);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
