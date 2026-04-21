using Microsoft.EntityFrameworkCore;
using API.Models;
using API.Models.Address;

namespace API.Data
{
      public class AppDbContext : DbContext
      {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<User> Users { get; set; }
            public DbSet<UserToken> UserTokens { get; set; }
            public DbSet<Role> Roles { get; set; }
            public DbSet<AuditTrail> AuditTrails { get; set; }
            public DbSet<ErrorLog> ErrorLogs { get; set; }
            public DbSet<Region> Regions { get; set; }
            public DbSet<Province> Provinces { get; set; }
            public DbSet<CityMunicipality> CitiesMunicipalities { get; set; }
            public DbSet<Barangay> Barangays { get; set; }
            public DbSet<Gender> Genders { get; set; }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            }
      }
}