using API.Attributes;
using API.Constants;
using API.DTOs.Address;
using API.DTOs.Dropdown;
using API.Helpers.Pagination;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/address")]
    public class AddressController : BaseApiController
    {
        private readonly AddressService _addressService;

        public AddressController(AddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("region/all")]
        [AuditTrail(IsIgnore = true)]
        public async Task<ActionResult<List<RegionDto>>> GetRegions()
        {
            return await _addressService.GetRegionsAsync();
        }

        [HttpPost("region/all/paginate")]
        [AuditTrail(IsIgnore = true)]
        public async Task<ActionResult<PaginatedResult<DropdownDto>>> GetRegionsPaginated([FromBody] DropdownParamsDto paramsDto)
        {
            return await _addressService.GetRegionsPaginatedAsync(paramsDto.search, paramsDto.pageNumber, paramsDto.pageSize);
        }

        [HttpGet("provinces/{regionId}")]
        [AuditTrail(IsIgnore = true)]
        public async Task<ActionResult<List<ProvinceDto>>> GetProvincesByRegionId(int regionId)
        {
            return await _addressService.GetProvincesAsync(regionId);
        }

        [HttpGet("cities-municipalities/by-province/{provinceId}")]
        [AuditTrail(IsIgnore = true)]
        public async Task<ActionResult<List<CityMunicipalityDto>>> GetCitiesMunicipalitiesByProvince(int provinceId)
        {
            return await _addressService.GetCitiesMunicipalitiesAsync(provinceId);
        }

        [HttpGet("cities-municipalities/by-region/{regionId}")]
        [AuditTrail(IsIgnore = true)]
        public async Task<ActionResult<List<CityMunicipalityDto>>> GetCitiesMunicipalitiesByRegion(int regionId)
        {
            return await _addressService.GetCitiesMunicipalitiesAsync(regionId);
        }


        [HttpGet("barangays/{cityId}")]
        [AuditTrail(IsIgnore = true)]
        public async Task<ActionResult<List<BarangayDto>>> GetBarangaysAsync(int cityId)
        {
            return await _addressService.GetBarangaysAsync(cityId);
        }
    }
}
