using System;
using System.ComponentModel;
using API.Data;
using API.DTOs.Address;
using API.DTOs.Dropdown;
using API.Helpers.Pagination;
using API.Repositories.Interfaces;

namespace API.Services;

public class AddressService
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uow;

    public AddressService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<List<RegionDto>> GetRegionsAsync()
    {
        return await _uow.Addresses.GetRegionsAsync();
    }

    public async Task<List<ProvinceDto>> GetProvincesAsync(int regionId)
    {
        return await _uow.Addresses.GetProvincesAsync(regionId);
    }

    public async Task<List<CityMunicipalityDto>> GetCitiesMunicipalitiesAsync(int id, bool byRegion = false)
    {
        return await _uow.Addresses.GetCitiesMunicipalitiesAsync(id, byRegion);
    }

    public async Task<List<BarangayDto>> GetBarangaysAsync(int cityId)
    {
        return await _uow.Addresses.GetBarangaysAsync(cityId);
    }

    public async Task<PaginatedResult<DropdownDto>> GetRegionsPaginatedAsync(string? search, int pageNumber, int pageSize)
    {
        return await _uow.Addresses.GetRegionsPaginatedAsync(search, pageNumber, pageSize);
    }

    public async Task<PaginatedResult<DropdownDto>> GetProvincesPaginatedAsync(string? search, int pageNumber, int pageSize, int regionId)
    {
        return await _uow.Addresses.GetProvincesPaginatedAsync(search, pageNumber, pageSize, regionId);
    }

    public async Task<PaginatedResult<DropdownDto>> GetCitiesMunicipalitiesPaginatedAsync(string? search, int pageNumber, int pageSize, int id, bool byRegion = false)
    {
        return await _uow.Addresses.GetCitiesMunicipalitiesPaginatedAsync(search, pageNumber, pageSize, id, byRegion);
    }
    public async Task<PaginatedResult<DropdownDto>> GetBarangaysPaginatedAsync(string? search, int pageNumber, int pageSize, int cityMunicipalityId)
    {
        return await _uow.Addresses.GetBarangaysPaginatedAsync(search, pageNumber, pageSize, cityMunicipalityId);
    }

}
