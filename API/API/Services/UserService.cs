using System;
using API.Common;
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

        if (dto.RegionId != null)
        {
            addressSet = true;
            if (await _uow.Addresses.IsRegionExistsAsync(dto.RegionId.Value)) errors.Add("Region does not exists");
        }

        if (addressSet)
        {
            if (await _uow.Addresses.IsValidAddressAsync(dto.RegionId!.Value, dto.ProvinceId!.Value, dto.CityMunicipalityId!.Value, dto.BarangayId!.Value, false)) errors.Add("Region does not exists");
        }

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
