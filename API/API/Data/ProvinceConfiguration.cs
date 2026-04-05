using System;
using API.Models;
using API.Models.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data;

public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> entity)
    {
        entity.ToTable("Provinces");
        entity.HasKey(r => r.Id);
        entity.Property(r => r.Id).ValueGeneratedOnAdd();
        entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
        entity.Property(t => t.DateCreated).IsRequired(true);
        entity.Property(t => t.DateModified).IsRequired(false);
        entity.Property(r => r.CreatedBy).IsRequired(false);
        entity.Property(r => r.ModifiedBy).IsRequired(false);
        entity.Property(r => r.IsActive).HasDefaultValue(true);

        entity.HasOne(p => p.Region)
            .WithMany()
            .HasForeignKey(p => p.RegionId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
