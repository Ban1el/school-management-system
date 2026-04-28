using System;
using API.DTOs.Address;
using API.DTOs.Dropdown;
using API.Helpers.Pagination;
using API.Models.Address;

namespace API.Repositories.Interfaces;

public interface IAddressRepository
{
    Task<List<RegionDto>> GetRegionsAsync();
    Task<List<ProvinceDto>> GetProvincesAsync(int id);
    Task<List<CityMunicipalityDto>> GetCitiesMunicipalitiesAsync(int id, bool byRegion = false);
    Task<List<BarangayDto>> GetBarangaysAsync(int id);
    Task<PaginatedResult<DropdownDto>> GetRegionsPaginatedAsync(string? search, int pageNumber, int pageSize);
    Task<PaginatedResult<DropdownDto>> GetProvincesPaginatedAsync(string? search, int pageNumber, int pageSize, int regionId);
    Task<PaginatedResult<DropdownDto>> GetCitiesMunicipalitiesPaginatedAsync(string? search, int pageNumber, int pageSize, int id, bool byRegion);
    Task<PaginatedResult<DropdownDto>> GetBarangaysPaginatedAsync(string? search, int pageNumber, int pageSize, int cityId);
    Task<bool> IsRegionExistsAsync(int id);
    Task<bool> IsProvinceExistsAsync(int id);
    Task<bool> IsCityMunicipalityExistsAsync(int id);
    Task<bool> IsBarangayExistsAsync(int id);
    Task<bool> IsValidAddressAsync(int regionId, int provinceId, int cityMunicipalityId, int barangayId, bool isNcr);
}
