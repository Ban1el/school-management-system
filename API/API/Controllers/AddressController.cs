using API.Attributes;
using API.Constants;
using API.DTOs.Address;
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

        [HttpGet("regions")]
        [AuditTrail(IsIgnore = true)]
        public async Task<ActionResult<List<RegionDto>>> GetRegions()
        {
            var result = await _addressService.GetRegionsAsync();
            return Ok(result);
        }
    }
}
