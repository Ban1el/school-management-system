using System;
using API.Common;
using API.Constants;
using API.Data;
using API.DTOs.User;
using API.DTOs.Users;
using API.Repositories.Interfaces;

namespace API.Services;

public class UserService
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uow;

    public UserService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        return await _uow.Users.GetByIdAsync(id);
    }

    public async Task<UserIdentityDto?> GetUserIdentityAsync(int id)
    {
        return await _uow.Users.GetUserIdentityAsync(id);
    }

    public async Task<ServiceResult<UserDto?>> UpdateUserProfileAsync(int id, UserProfileUpdateDto dto)
    {
        var result = new ServiceResult<UserDto?>();

        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            if (await _uow.Users.IsEmailTakenAsync(dto.Email, id)) errors.Add("Email already taken");
        }

        bool addressSet = false;
        bool addressExists = true;
        bool isNcr = false;

        if (dto.RegionId != null)
        {
            addressSet = true;
            if (!await _uow.Addresses.IsRegionExistsAsync(dto.RegionId.Value))
            {
                errors.Add("Region does not exists");
                addressExists = false;
            }

            var region = await _uow.Addresses.GetRegionByIdAsync(dto.RegionId.Value);

            if (region!.Name == AddressConstants.NCR) isNcr = true;
        }

        if (dto.ProvinceId != null)
        {
            addressSet = true;
            if (!await _uow.Addresses.IsProvinceExistsAsync(dto.ProvinceId.Value))
            {
                errors.Add("Province does not exists");
                addressExists = false;
            }
        }

        if (dto.CityMunicipalityId != null)
        {
            addressSet = true;
            if (!await _uow.Addresses.IsCityMunicipalityExistsAsync(dto.CityMunicipalityId.Value))
            {
                errors.Add("City / Municipality does not exists");
                addressExists = false;
            }
        }

        if (dto.BarangayId != null)
        {
            addressSet = true;
            if (!await _uow.Addresses.IsBarangayExistsAsync(dto.BarangayId.Value))
            {
                errors.Add("Barangay does not exists");
                addressExists = false;
            }

        }

        if (addressSet && addressExists)
        {
            bool addressComplete = true;
            if (!dto.RegionId.HasValue) addressComplete = false;
            if (!dto.ProvinceId.HasValue && !isNcr) addressComplete = false;
            if (!dto.CityMunicipalityId.HasValue) addressComplete = false;
            if (!dto.BarangayId.HasValue) addressComplete = false;

            if (!addressComplete) errors.Add("Incomplete address");
            else
            {
                if (await _uow.Addresses.IsValidAddressAsync(dto.RegionId!.Value, dto.ProvinceId, dto.CityMunicipalityId!.Value, dto.BarangayId!.Value, isNcr)) errors.Add("Invalid address");
            }
        }

        if (errors.Count() > 0) return ServiceResult<UserDto?>.Fail(errors);

        await _uow.BeginTransactionAsync();

        try
        {
            var user = await _uow.Users.UpdateUserProfileAsync(id, dto);
            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();

            if (user != null) result.Data = user;

            return result;
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }
}
