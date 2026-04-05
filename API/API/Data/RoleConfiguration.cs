using System;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> entity)
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
        .IsRequired(false);
        entity.Property(r => r.ModifiedBy)
        .IsRequired(false);
        entity.Property(r => r.IsActive)
        .HasDefaultValue(true);
    }
}
