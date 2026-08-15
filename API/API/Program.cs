using API.Data.Seed;
using API.Extensions;
using API.Middleware;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, logger) =>
{
    logger
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.MSSqlServer(
            connectionString: context.Configuration.GetConnectionString("SMSDatabase")!,
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = true
            })
        .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 90,
        fileSizeLimitBytes: 10_000_000,
        rollOnFileSizeLimit: true);
});

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddOptionsConfiguration(builder.Configuration);

builder.Services.AddOpenApi();

//Idempotency
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IdempotencyFilter>();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();

using (var scope = app.Services.CreateScope())
{
    UserSeedData.Initialize(scope.ServiceProvider);
    await AddressSeedData.InitializeAsync(scope.ServiceProvider);
    await GenderSeedData.InitializeAsync(scope.ServiceProvider);
}

app.UseCors(x => x.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("https://localhost:4200"));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<AuditTrailMiddleware>();

app.MapControllers();

app.Run();