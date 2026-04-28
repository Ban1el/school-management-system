using API.Attributes;
using API.Constants;
using API.DTOs.User;
using API.DTOs.Users;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        [AuditTrail(IsIgnore = true)]
        public async Task<ActionResult<UserDto?>> GetById(int id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        [HttpPut("profile/{id}")]
        [AuditTrail(Module = ModuleConstants.User, Action = ActionConstants.Update)]
        public async Task<ActionResult<UserDto?>> UpdateProfile(int id, [FromBody] UserProfileUpdateDto dto)
        {
            var result = await _userService.UpdateUserProfileAsync(id, dto);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            HttpContext.Items[AuditTrailConstants.ReferenceId] = result.Data?.Id;

            return Ok(result.Data);
        }
    }
}
