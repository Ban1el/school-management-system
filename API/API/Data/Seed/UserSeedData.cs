using System;
using API.Data;
using API.Models;
using API.Utilities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Seed;

public class UserSeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new AppDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<AppDbContext>>()))
        {
            if (context == null || context.Users == null || context.Roles == null)
            {
                throw new ArgumentNullException("Null DBContext");
            }

            CryptoUtils _crypto = new CryptoUtils();
            string passwordSalt = _crypto.GenerateSalt();

            var adminRole = context.Roles.FirstOrDefault(r => r.Name == "admin");
            if (adminRole == null)
            {
                adminRole = new Role
                {
                    Name = "admin",
                    DateCreated = DateTime.Now,
                    CreatedBy = null,
                    ModifiedBy = null,
                    IsActive = true
                };
                context.Roles.Add(adminRole);
                context.SaveChanges();
            }

            if (!context.Users.Any(u => u.Username == "admin"))
            {
                var adminUser = new User
                {
                    RoleId = adminRole.Id,
                    Username = "admin",
                    Email = "test@test.com",
                    Password = _crypto.HashPassword("admin@123", passwordSalt),
                    PasswordSalt = passwordSalt,
                    DateCreated = DateTime.Now,
                    CreatedBy = null,
                    ModifiedBy = null,
                    IsActive = true
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}
