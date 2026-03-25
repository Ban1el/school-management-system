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
    }
}
