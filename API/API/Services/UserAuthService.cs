using API.DTOs.Users;
using API.Common;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Utilities;

namespace API.Services;

public class UserAuthService
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public UserAuthService(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<ServiceResult<SignInResponseDto>> SignInAsync(UserSigninDto dto)
    {
        var result = new ServiceResult<SignInResponseDto>();
        CryptoUtils _crypto = new CryptoUtils();

        var user = await _context.Users
            .Where(u => u.Username == dto.Username)
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

        if (user == null)
            return ServiceResult<SignInResponseDto>.Fail("Invalid username.");

        if (user.IsActive == false)
            return ServiceResult<SignInResponseDto>.Fail("Invalid user.");

        if (user.Password != _crypto.HashPassword(dto.Password, user.PasswordSalt))
            return ServiceResult<SignInResponseDto>.Fail("Invalid password.");

        var signInResponse = new SignInResponseDto
        {
            accesstoken = _tokenService.CreateToken(user),
            refreshtoken = await _tokenService.SetRefreshToken(user.Id),
        };

        return new ServiceResult<SignInResponseDto>
        {
            Success = true,
            Data = signInResponse
        };
    }

    public async Task<ServiceResult<SignInResponseDto>> RefreshAccessTokenAsync(string refreshToken)
    {
        var userToken = await _context.UserTokens
            .Where(u => u.RefreshToken == refreshToken)
            .Select(u => new { u.ExpiryDate, u.UserId })
            .FirstOrDefaultAsync();

        if (userToken == null || userToken.ExpiryDate <= DateTime.UtcNow)
            throw new UnauthorizedAccessException();

        var user = await _context.Users
            .Where(u => u.Id == userToken.UserId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                RoleId = u.RoleId,
                IsActive = u.IsActive
            })
            .FirstOrDefaultAsync();

        if (user == null || user.IsActive == false)
            throw new UnauthorizedAccessException();

        var signInResponse = new SignInResponseDto
        {
            accesstoken = _tokenService.CreateToken(user),
            refreshtoken = refreshToken,
        };

        return new ServiceResult<SignInResponseDto>
        {
            Success = true,
            Data = signInResponse
        };
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var userToken = await _context.UserTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if (userToken == null) return;

            userToken.RefreshToken = null;
            userToken.ExpiryDate = null;
            userToken.DateModified = DateTime.UtcNow;

            _context.UserTokens.Update(userToken);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
