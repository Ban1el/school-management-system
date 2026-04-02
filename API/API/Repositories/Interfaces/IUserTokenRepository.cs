using System;
using API.DTOs.UserToken;

namespace API.Repositories.Interfaces;

public interface IUserTokenRepository
{
    Task<UserTokenDto?> GetByRefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
    Task UpsertTokenAsync(int userId, string refreshToken, DateTime expiryDate);
}
