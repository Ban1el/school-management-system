using System;
using API.DTOs.Address;
using API.DTOs.Dropdown;
using API.Helpers.Pagination;
using API.Models.Address;

namespace API.Repositories.Interfaces;

public interface IAddressRepository
{
    Task<List<RegionDto>> GetRegionsAsync();
    Task<PaginatedResult<DropdownDto>> GetRegionsPaginatedAsync(string? search, int pageNumber, int pageSize);
    Task<List<ProvinceDto>> GetProvincesAsync(int id);
    Task<List<CityMunicipalityDto>> GetCitiesMunicipalitiesAsync(int id, bool byRegion = false);
    Task<List<BarangayDto>> GetBarangaysAsync(int id);
}
