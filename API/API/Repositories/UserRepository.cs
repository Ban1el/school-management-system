using System;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.DTOs.Users;
using API.Repositories.Interfaces;

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

}
