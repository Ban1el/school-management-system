using System;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Seed;

public class GenderSeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using (var context = new AppDbContext(
                    serviceProvider.GetRequiredService<
                        DbContextOptions<AppDbContext>>()))
        {
            if (context == null || context.Genders == null)
            {
                throw new ArgumentNullException("Null DBContext");
            }

            if (context.Genders.Any()) return;

            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                context.Genders.AddRange(
                    new Gender { Name = "Male" },
                    new Gender { Name = "Female" }
                );

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
