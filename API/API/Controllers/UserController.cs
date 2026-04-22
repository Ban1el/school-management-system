using API.Attributes;
using API.DTOs.Users;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var user = await _userService.GetUserByIdAsync(id);

            return user;
        }
    }
}
