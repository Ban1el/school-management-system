using API.DTOs.Gender;
using API.DTOs.Role;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/role")]
    public class RoleController : BaseApiController
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("all/active")]
        public async Task<ActionResult<List<RoleDto>>> GetRolesActive()
        {
            return await _roleService.GetRolesActiveAsync();
        }
    }
}
