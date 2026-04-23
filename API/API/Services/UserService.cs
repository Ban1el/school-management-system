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

    public async Task<UserIdentityDto?> GetUserIdentity(int id)
    {
        return await _uow.Users.GetUserIdentityAsync(id);
    }

}
