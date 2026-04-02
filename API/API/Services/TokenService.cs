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
using API.Repositories.Interfaces;

namespace API.Services;

public class TokenService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly IUnitOfWork _uow;

    public TokenService(IUnitOfWork uow, AppDbContext context, IConfiguration config)
    {
        _uow = uow;
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
            // Expires = DateTime.Now.AddSeconds(5),
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
        string refreshToken = "";
        refreshToken = _crypto.GenerateRandomKey();
        DateTime expiryDate = DateTime.UtcNow.AddDays(Convert.ToInt32(_config["TokenSettings:RefreshTokenExpiryDays"]));

        await _uow.BeginTransactionAsync();

        try
        {
            await _uow.UserTokens.UpsertTokenAsync(userId, refreshToken, expiryDate);
            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }

        return refreshToken;
    }
}
