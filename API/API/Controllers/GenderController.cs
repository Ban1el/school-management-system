using API.Attributes;
using API.DTOs.Gender;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/gender")]
    public class GenderController : BaseApiController
    {
        private readonly GenderService _genderService;

        public GenderController(GenderService genderService)
        {
            _genderService = genderService;
        }

        [HttpGet("gender/all/active")]
        [AuditTrail(IsIgnore = true)]
        public async Task<ActionResult<List<GenderDto>>> GetGendersActive()
        {
            return await _genderService.GetGendersActiveAsync();
        }
    }
}
