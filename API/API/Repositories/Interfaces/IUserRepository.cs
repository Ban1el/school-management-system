using System;
using API.DTOs.User;
using API.DTOs.Users;

namespace API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<UserDto?> GetByUsernameAsync(string username);
    Task<UserDto?> GetByIdAsync(int userId);
    Task<UserIdentityDto?> GetUserIdentityAsync(int userId);
}
