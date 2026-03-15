using System;
using API.Data;
using API.Utilities;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public class SeedData
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

            var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
            if (adminRole == null)
            {
                adminRole = new Role
                {
                    Name = "Admin",
                    DateCreated = DateTime.Now,
                    CreatedBy = 0,
                    ModifiedBy = 0,
                    IsActive = true
                };
                context.Roles.Add(adminRole);
                context.SaveChanges();
            }

            if (!context.Users.Any(u => u.Username == "Admin"))
            {
                var adminUser = new User
                {
                    RoleId = adminRole.Id,
                    Username = "Admin",
                    Email = "test@test.com",
                    Password = _crypto.HashPassword("admin@123", passwordSalt),
                    PasswordSalt = passwordSalt,
                    DateCreated = DateTime.Now,
                    CreatedBy = 0,
                    ModifiedBy = 0,
                    IsActive = true
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}
