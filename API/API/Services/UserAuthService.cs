using API.DTOs.Users;
using API.Common;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Utilities;
using API.Repositories.Interfaces;

namespace API.Services;

public class UserAuthService
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;
    private readonly IUnitOfWork _uow;

    public UserAuthService(IUnitOfWork uow, AppDbContext context, TokenService tokenService)
    {
        _uow = uow;
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<ServiceResult<SignInResponseDto>> SignInAsync(UserSigninDto dto)
    {
        var result = new ServiceResult<SignInResponseDto>();
        CryptoUtils _crypto = new CryptoUtils();

        var user = await _uow.Users.GetByUsernameAsync(dto.Username);

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
        var userToken = await _uow.UserTokens.GetByRefreshTokenAsync(refreshToken);

        if (userToken == null || userToken.ExpiryDate <= DateTime.UtcNow)
            throw new UnauthorizedAccessException();

        var user = await _uow.Users.GetByIdAsync(userToken.UserId);

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
        await _uow.BeginTransactionAsync();

        try
        {
            await _uow.UserTokens.RevokeRefreshTokenAsync(refreshToken);
            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }
}
