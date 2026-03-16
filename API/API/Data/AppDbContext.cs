using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
      public class AppDbContext : DbContext
      {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<User> Users { get; set; }
            public DbSet<UsersToken> UserTokens { get; set; }
            public DbSet<Role> Roles { get; set; }
            public DbSet<AuditTrail> AuditTrails { get; set; }
            public DbSet<ErrorLog> ErrorLogs { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  modelBuilder.Entity<Role>(entity =>
                  {
                        entity.ToTable("Roles");
                        entity.HasKey(r => r.Id);
                        entity.Property(r => r.Id).ValueGeneratedOnAdd();
                        entity.Property(r => r.Name)
                        .IsRequired()
                        .HasMaxLength(50);

                        entity.Property(r => r.DateCreated)
                        .IsRequired();
                        entity.Property(r => r.DateModified)
                        .IsRequired(false);
                        entity.Property(r => r.CreatedBy)
                        .IsRequired();
                        entity.Property(r => r.ModifiedBy)
                        .IsRequired();
                        entity.Property(r => r.IsActive)
                        .HasDefaultValue(true);
                  });

                  modelBuilder.Entity<User>(entity =>
                  {
                        entity.ToTable("Users");
                        entity.HasKey(u => u.Id);
                        entity.Property(u => u.Id).ValueGeneratedOnAdd();
                        entity.Property(u => u.Username)
                        .IsRequired()
                        .HasMaxLength(50);
                        entity.Property(u => u.Email)
                        .IsRequired()
                        .HasMaxLength(100);
                        entity.Property(u => u.Password)
                        .IsRequired();
                        entity.Property(u => u.PasswordSalt)
                        .IsRequired();
                        entity.Property(u => u.DateCreated)
                        .IsRequired();
                        entity.Property(u => u.DateModified)
                        .IsRequired(false);
                        entity.Property(u => u.CreatedBy)
                        .IsRequired();
                        entity.Property(u => u.ModifiedBy)
                        .IsRequired();
                        entity.Property(u => u.IsActive)
                        .HasDefaultValue(true);

                        entity.HasOne<Role>()
                        .WithMany()
                        .HasForeignKey(u => u.RoleId)
                        .OnDelete(DeleteBehavior.Restrict);

                        entity.HasIndex(u => new { u.Email, u.Username })
                        .IsUnique();
                  });

                  modelBuilder.Entity<UsersToken>(entity =>
                  {
                        entity.ToTable("UsersTokens");
                        entity.HasKey(t => t.Id);
                        entity.Property(t => t.Id).ValueGeneratedOnAdd();
                        entity.Property(t => t.AccessToken)
                        .IsRequired();
                        entity.Property(t => t.RefreshToken)
                        .IsRequired();
                        entity.Property(t => t.ExpiryDate)
                        .IsRequired();
                        entity.Property(t => t.DateModified)
                        .IsRequired(false);

                        entity.HasOne<User>()
                        .WithMany()
                        .HasForeignKey(t => t.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
                  });

                  modelBuilder.Entity<AuditTrail>(entity =>
                  {
                        entity.ToTable("AuditTrails");
                        entity.HasKey(t => t.Id);
                        entity.Property(t => t.Id).ValueGeneratedOnAdd();

                        entity.Property(t => t.Module)
                              .IsRequired()
                              .HasMaxLength(100);
                        entity.Property(t => t.Action)
                              .IsRequired()
                              .HasMaxLength(100);
                        entity.Property(t => t.Data)
                              .HasMaxLength(1000);
                        entity.Property(t => t.IpAddress)
                              .HasMaxLength(50);
                        entity.Property(t => t.ReqIpAddress)
                              .HasMaxLength(50);
                        entity.Property(t => t.Path)
                              .HasMaxLength(255);
                        entity.Property(t => t.RefId)
                              .HasMaxLength(100);
                        entity.Property(t => t.DateCreated)
                              .IsRequired();
                        entity.Property(t => t.IsRequest)
                              .IsRequired();
                  });

                  modelBuilder.Entity<ErrorLog>(entity =>
                  {
                        entity.ToTable("ErrorLogs");
                        entity.HasKey(e => e.Id);
                        entity.Property(e => e.Id).ValueGeneratedOnAdd();
                        entity.Property(e => e.Description)
                              .IsRequired()
                              .HasMaxLength(1000);
                        entity.Property(e => e.IpAddress)
                              .HasMaxLength(50);
                        entity.Property(e => e.DateCreated)
            .IsRequired();
                  });
            }
      }
}