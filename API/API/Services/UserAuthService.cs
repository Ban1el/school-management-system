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
            token = _tokenService.CreateToken(user)
        };

        return new ServiceResult<SignInResponseDto>
        {
            Success = true,
            Data = signInResponse
        };
    }
}
