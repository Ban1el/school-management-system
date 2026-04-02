using System;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.DTOs.Users;
using API.Repositories.Interfaces;

namespace API.Repositories;

public class UserRepository(AppDbContext _context) : IUserRepository
{
    public async Task<UserDto?> GetByIdAsync(int userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                RoleId = u.RoleId,
                IsActive = u.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UserDto?> GetByUsernameAsync(string username)
    {
        return await _context.Users
             .Where(u => u.Username == username)
             .Select(u => new UserDto
             {
                 Id = u.Id,
                 Email = u.Email,
                 RoleId = u.RoleId,
                 IsActive = u.IsActive,
                 Password = u.Password,
                 PasswordSalt = u.PasswordSalt
             })
             .FirstOrDefaultAsync();
    }
}
