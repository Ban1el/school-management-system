using System;
using API.Models;
using API.Models.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data;

public class CityConfiguration : IEntityTypeConfiguration<CityMunicipality>
{
    public void Configure(EntityTypeBuilder<CityMunicipality> entity)
    {
        entity.ToTable("CitiesMunicipalities");
        entity.HasKey(c => c.Id);
        entity.Property(c => c.Id).ValueGeneratedOnAdd();
        entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
        entity.Property(c => c.DateCreated).IsRequired(true);
        entity.Property(c => c.DateModified).IsRequired(false);
        entity.Property(c => c.CreatedBy).IsRequired(false);
        entity.Property(c => c.ModifiedBy).IsRequired(false);
        entity.Property(c => c.IsActive).HasDefaultValue(true);

        entity.HasOne(c => c.Region)
           .WithMany()
           .HasForeignKey(c => c.RegionId)
           .IsRequired(false)
           .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(c => c.Province)
            .WithMany()
            .HasForeignKey(c => c.ProvinceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
