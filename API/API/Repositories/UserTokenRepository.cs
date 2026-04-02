using System;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Repositories.Interfaces;
using API.DTOs.UserToken;
using API.Models;
using Microsoft.JSInterop;

namespace API.Repositories;

public class UserTokenRepository(AppDbContext _context) : IUserTokenRepository
{
    public async Task<UserTokenDto?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.UserTokens
             .Where(u => u.RefreshToken == refreshToken)
             .Select(u => new UserTokenDto
             {
                 Id = u.Id,
                 UserId = u.UserId,
                 RefreshToken = u.RefreshToken,
                 ExpiryDate = u.ExpiryDate,
                 DateCreated = u.DateCreated,
                 DateModified = u.DateModified

             })
             .FirstOrDefaultAsync();
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var userToken = await _context.UserTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

        if (userToken == null) return;

        userToken.RefreshToken = null;
        userToken.ExpiryDate = null;
        userToken.DateModified = DateTime.UtcNow;
    }

    public async Task UpsertTokenAsync(int userId, string refreshToken, DateTime expiryDate)
    {
        var existing = await _context.UserTokens
            .FirstOrDefaultAsync(t => t.UserId == userId);

        if (existing != null)
        {
            existing.RefreshToken = refreshToken;
            existing.ExpiryDate = expiryDate;
            existing.DateModified = DateTime.UtcNow;
        }
        else
        {
            await _context.UserTokens.AddAsync(new UserToken
            {
                UserId = userId,
                RefreshToken = refreshToken,
                ExpiryDate = expiryDate,
            });
        }
    }
}
