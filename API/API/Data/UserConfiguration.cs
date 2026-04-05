using System;
using API.Models;
using API.Models.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
     public void Configure(EntityTypeBuilder<User> entity)
     {
          entity.ToTable("Users");
          entity.HasKey(u => u.Id);
          entity.Property(u => u.Id).ValueGeneratedOnAdd();
          entity.Property(u => u.FirstName).IsRequired(false).HasMaxLength(50);
          entity.Property(u => u.MiddleName).IsRequired(false).HasMaxLength(50);
          entity.Property(u => u.LastName).IsRequired(false).HasMaxLength(50);
          entity.Property(u => u.MobileNumber).IsRequired(false).HasMaxLength(11);
          entity.Property(u => u.StreetAddress).IsRequired(false).HasMaxLength(255);
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
     .IsRequired(false);
          entity.Property(u => u.ModifiedBy)
     .IsRequired(false);
          entity.Property(u => u.IsActive)
     .HasDefaultValue(true);

          entity.HasOne<Role>()
     .WithMany()
     .HasForeignKey(u => u.RoleId)
     .OnDelete(DeleteBehavior.Restrict);

          entity.HasOne<Region>()
     .WithMany()
     .HasForeignKey(u => u.RegionId)
     .IsRequired(false)
     .OnDelete(DeleteBehavior.Restrict);

          entity.HasOne<Province>()
      .WithMany()
      .HasForeignKey(u => u.ProvinceId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.Restrict);

          entity.HasOne<CityMunicipality>()
     .WithMany()
     .HasForeignKey(u => u.CityMunicipalityId)
     .IsRequired(false)
     .OnDelete(DeleteBehavior.Restrict);


          entity.HasOne<Barangay>()
     .WithMany()
     .HasForeignKey(u => u.BarangayId)
     .IsRequired(false)
     .OnDelete(DeleteBehavior.Restrict);

          entity.HasIndex(u => new { u.Email, u.Username })
     .IsUnique();
     }
}
