using System;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data;

public class GenderConfiguration : IEntityTypeConfiguration<Gender>
{
    public void Configure(EntityTypeBuilder<Gender> entity)
    {
        entity.ToTable("Genders");
        entity.HasKey(g => g.Id);
        entity.Property(g => g.Id).ValueGeneratedOnAdd();
        entity.Property(g => g.Name).IsRequired().HasMaxLength(50);
        entity.Property(t => t.DateCreated).IsRequired(true);
        entity.Property(t => t.DateModified).IsRequired(false);
        entity.Property(r => r.CreatedBy).IsRequired(false);
        entity.Property(r => r.ModifiedBy).IsRequired(false);
        entity.Property(r => r.IsActive).HasDefaultValue(true);
    }
}
