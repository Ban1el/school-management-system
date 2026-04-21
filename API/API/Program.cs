using API.Data.Seed;
using API.Middleware;
using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    UserSeedData.Initialize(scope.ServiceProvider);
    await AddressSeedData.InitializeAsync(scope.ServiceProvider);
    await GenderSeedData.InitializeAsync(scope.ServiceProvider);
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<AuditTrailMiddleware>();

app.MapControllers();

app.Run();
