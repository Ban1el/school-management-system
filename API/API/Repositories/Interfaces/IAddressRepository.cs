using System;
using API.DTOs.Address;
using API.Models.Address;

namespace API.Repositories.Interfaces;

public interface IAddressRepository
{
    Task<List<RegionDto>> GetRegionsAsync();
    Task<List<ProvinceDto>> GetProvincesAsync(int id);
    Task<List<CityMunicipalityDto>> GetCitiesMunicipalitiesAsync(int id, bool isNcr = false);
    Task<List<BarangayDto>> GetBarangaysAsync(int id);
}
