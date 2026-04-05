using System;
using API.Models;
using API.Models.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data;

public class BarangayConfiguration : IEntityTypeConfiguration<Barangay>
{
    public void Configure(EntityTypeBuilder<Barangay> entity)
    {
        entity.ToTable("Barangays");
        entity.HasKey(b => b.Id);
        entity.Property(b => b.Id).ValueGeneratedOnAdd();
        entity.Property(b => b.Name).IsRequired().HasMaxLength(100);
        entity.Property(b => b.DateCreated).IsRequired(true);
        entity.Property(b => b.DateModified).IsRequired(false);
        entity.Property(b => b.CreatedBy).IsRequired(false);
        entity.Property(b => b.ModifiedBy).IsRequired(false);
        entity.Property(b => b.IsActive).HasDefaultValue(true);

        entity.HasOne<CityMunicipality>()
            .WithMany()
            .HasForeignKey(b => b.CityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}