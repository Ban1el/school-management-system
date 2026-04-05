using System;
using API.Models;
using API.Models.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace API.Data;

public class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> entity)
    {
        entity.ToTable("Regions");
        entity.HasKey(r => r.Id);
        entity.Property(r => r.Id).ValueGeneratedOnAdd();
        entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
        entity.Property(t => t.DateCreated).IsRequired(true);
        entity.Property(t => t.DateModified).IsRequired(false);
        entity.Property(r => r.CreatedBy).IsRequired(false);
        entity.Property(r => r.ModifiedBy).IsRequired(false);
        entity.Property(r => r.IsActive).HasDefaultValue(true);
    }
}
