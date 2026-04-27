using System;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.DTOs.Users;
using API.Repositories.Interfaces;
using API.DTOs.User;

namespace API.Repositories;

public class UserRepository(AppDbContext _context) : IUserRepository
{
    public async Task<UserDto?> GetByIdAsync(int userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.Role)
            .Include(u => u.Region)
            .Include(u => u.Province)
            .Include(u => u.CityMunicipality)
            .Include(u => u.Barangay)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Username = u.Username,
                RoleId = u.RoleId,
                RoleName = u.Role.Name,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                MobileNumber = u.MobileNumber,
                StreetAddress = u.StreetAddress,
                ZipCode = u.ZipCode,
                RegionId = u.RegionId,
                RegionName = u.Region != null ? u.Region.Name : null,
                ProvinceId = u.ProvinceId,
                ProvinceName = u.Province != null ? u.Province.Name : null,
                CityMunicipalityId = u.CityMunicipalityId,
                CityMunicipalityName = u.CityMunicipality != null ? u.CityMunicipality.Name : null,
                BarangayId = u.BarangayId,
                BarangayName = u.Barangay != null ? u.Barangay.Name : null,
                IsActive = u.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UserDto?> GetByUsernameAsync(string username)
    {
        return await _context.Users
             .Where(u => u.Username == username)
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
    }

    public async Task<UserIdentityDto?> GetUserIdentityAsync(int userId)
    {
        return await _context.Users
           .Select(u => new UserIdentityDto
           {
               Id = u.Id,
               Username = u.Username
           })
           .FirstOrDefaultAsync();
    }

    public async Task<UserDto?> UpdateUserProfileAsync(int userId, UserProfileUpdateDto dto)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Region)
            .Include(u => u.Province)
            .Include(u => u.CityMunicipality)
            .Include(u => u.Barangay)
       .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return null;

        user.FirstName = string.IsNullOrWhiteSpace(dto.FirstName) ? user.FirstName : dto.FirstName;
        user.MiddleName = string.IsNullOrWhiteSpace(dto.MiddleName) ? user.MiddleName : dto.MiddleName;
        user.LastName = string.IsNullOrWhiteSpace(dto.LastName) ? user.LastName : dto.LastName;
        user.MobileNumber = string.IsNullOrWhiteSpace(dto.MobileNumber) ? user.MobileNumber : dto.MobileNumber;
        user.Email = string.IsNullOrWhiteSpace(dto.Email) ? user.Email : dto.Email;
        user.StreetAddress = string.IsNullOrWhiteSpace(dto.StreetAddress) ? user.StreetAddress : dto.StreetAddress;
        user.ZipCode = string.IsNullOrWhiteSpace(dto.ZipCode) ? user.ZipCode : dto.ZipCode;
        user.BarangayId = dto.BarangayId ?? user.BarangayId;
        user.RegionId = dto.RegionId ?? user.RegionId;
        user.ProvinceId = dto.ProvinceId ?? user.ProvinceId;
        user.CityMunicipalityId = dto.CityMunicipalityId ?? user.CityMunicipalityId;

        return new UserDto
        {
            Id = user.Id,
            RoleName = user.Role.Name,
            RoleId = user.Role.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            MobileNumber = user.MobileNumber,
            BarangayId = user.Barangay?.Id,
            BarangayName = user.Barangay?.Name,
            CityMunicipalityId = user.CityMunicipality?.Id,
            CityMunicipalityName = user.CityMunicipality?.Name,
            ProvinceId = user.Province?.Id,
            ProvinceName = user.Province?.Name,
            RegionId = user.Region?.Id,
            RegionName = user.Region?.Name,
            StreetAddress = user.StreetAddress,
            ZipCode = user.ZipCode,
            IsActive = user.IsActive,
        };
    }
}
