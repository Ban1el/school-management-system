using System;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> entity)
    {
        entity.ToTable("UserTokens");
        entity.HasKey(t => t.Id);
        entity.Property(t => t.Id).ValueGeneratedOnAdd();
        entity.Property(t => t.RefreshToken).IsRequired(false);
        entity.Property(t => t.ExpiryDate).IsRequired(false);
        entity.Property(t => t.DateCreated).IsRequired(true);
        entity.Property(t => t.DateModified)
        .IsRequired(false);

        entity.HasOne<User>()
        .WithMany()
        .HasForeignKey(t => t.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
