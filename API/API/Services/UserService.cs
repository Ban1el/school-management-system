using System;
using API.Data;
using API.DTOs.User;
using API.DTOs.Users;
using API.Repositories.Interfaces;

namespace API.Services;

public class UserService
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uow;

    public UserService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        return await _uow.Users.GetByIdAsync(id);
    }

    public async Task<UserIdentityDto?> GetUserIdentityAsync(int id)
    {
        return await _uow.Users.GetUserIdentityAsync(id);
    }

    public async Task<UserDto?> UpdateUserProfileAsync(int id, UserProfileUpdateDto dto)
    {
        await _uow.BeginTransactionAsync();

        try
        {
            var user = await _uow.Users.UpdateUserProfileAsync(id, dto);
            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();
            return user;
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }
}
