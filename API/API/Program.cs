using API.Data.Seed;
using API.Middleware;
using API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    UserSeedData.Initialize(scope.ServiceProvider);
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<AuditTrailMiddleware>();

app.MapControllers();

app.Run();
