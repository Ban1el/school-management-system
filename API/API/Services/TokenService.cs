using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Extensions;
using Microsoft.IdentityModel.Tokens;
using API.DTOs.Users;
using API.Data;
using API.Utilities;
using API.Models;

namespace API.Services;

public class TokenService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public TokenService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public string CreateToken(UserDto user)
    {
        var tokenKey = _config["JWT:Key"] ?? throw new Exception("Cannot Access! Token Key is not found.");

        if (tokenKey.Length < 64) throw new Exception("Token Key needs to be longer.");

        var issuer = _config["JWT:Issuer"];
        var audience = _config["JWT:Audience"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var claims = new List<Claim>
       {
           new(ClaimTypes.NameIdentifier, user.Id.ToString().Encrypt()),
           new(ClaimTypes.Name, user.Email.Encrypt()),
           new(ClaimTypes.Role, user.RoleId.ToString()),
       };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.Now.AddMinutes(Convert.ToInt32(_config["TokenSettings:AccessTokenExpiryMinutes"])),
            // Expires = DateTime.Now.AddSeconds(1),
            IssuedAt = DateTime.Now,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return jwtToken.ToString();
    }

    public async Task<string> SetRefreshToken(int userId)
    {
        CryptoUtils _crypto = new CryptoUtils();

        using var transaction = await _context.Database.BeginTransactionAsync();

        string refreshToken = "";

        try
        {
            var currentUserToken = _context.UserTokens.Where(t => t.UserId == userId).FirstOrDefault();

            refreshToken = _crypto.GenerateRandomKey();

            DateTime expiryDate = DateTime.UtcNow.AddDays(Convert.ToInt32(_config["TokenSettings:RefreshTokenExpiryDays"]));

            if (currentUserToken != null)
            {
                currentUserToken.RefreshToken = refreshToken;
                currentUserToken.ExpiryDate = expiryDate;
                // currentUserToken.ExpiryDate = DateTime.UtcNow.AddSeconds(1);
                currentUserToken.DateModified = DateTime.UtcNow;
                await _context.UserTokens.AddAsync(currentUserToken);
                _context.UserTokens.Update(currentUserToken);
            }
            else
            {
                var userTokenCreate = new UserToken
                {
                    UserId = userId,
                    RefreshToken = refreshToken,
                    // ExpiryDate = expiryDate;
                    ExpiryDate = DateTime.UtcNow.AddSeconds(1),
                    DateCreated = DateTime.UtcNow,
                    DateModified = null,
                };
                await _context.UserTokens.AddAsync(userTokenCreate);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return refreshToken;
    }
}
