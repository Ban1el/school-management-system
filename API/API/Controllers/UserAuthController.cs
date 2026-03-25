using API.Attributes;
using API.Constants;
using API.DTOs.Users;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/users/auth")]
    public class UserAuthController : BaseApiController
    {
        private readonly UserAuthService _userAuthService;
        public UserAuthController(UserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        [HttpPost("signin")]
        [AuditTrail(Module = ModuleConstants.UserAuthentication, Action = ActionConstants.Login)]
        public async Task<ActionResult<SignInResponseDto>> SignIn([FromBody] UserSigninDto dto)
        {
            var result = await _userAuthService.SignInAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }
    }
}
