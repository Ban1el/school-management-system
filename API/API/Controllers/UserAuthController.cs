using API.Attributes;
using API.Constants;
using API.DTOs.User;
using API.DTOs.Users;
using API.Extensions;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/user/auth")]
    public class UserAuthController : BaseApiController
    {
        private readonly IConfiguration _config;
        private readonly UserAuthService _userAuthService;
        private readonly UserService _userService;
        private readonly TokenService _tokenService;
        public UserAuthController(UserAuthService userAuthService, IConfiguration config, TokenService tokenService, UserService userService)
        {
            _userAuthService = userAuthService;
            _config = config;
            _tokenService = tokenService;
            _userService = userService;
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

            await SetRefreshTokenCookie(result.Data!.refreshtoken);
            await SetAccessTokenCookie(result.Data!.accesstoken);

            var userIdentity = await _userService.GetUserIdentity(result.Data.userId);

            return Ok(userIdentity);
        }

        [HttpPost("logout")]
        [AuditTrail(Module = ModuleConstants.UserAuthentication, Action = ActionConstants.Logout)]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _userAuthService.RevokeRefreshTokenAsync(refreshToken);
            }

            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            Response.Cookies.Delete("accessToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return NoContent();
        }

        [HttpPost("refresh/token")]
        [AuditTrail(IsIgnore = true)]
        public async Task<ActionResult<SignInResponseDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null) return Unauthorized();

            var result = await _userAuthService.RefreshAccessTokenAsync(refreshToken);

            if (result.Data != null) await SetAccessTokenCookie(result.Data.accesstoken);

            return Ok(result.Data);
        }

        [HttpGet("verify")]
        [Authorize]
        [AuditTrail(IsIgnore = true)]
        public async Task<ActionResult<UserIdentityDto>> Verify()
        {
            var userIdentity = await _userService.GetUserIdentity(User.GetUserId());
            return Ok(userIdentity);
        }

        private async Task SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(_config["TokenSettings:RefreshTokenExpiryDays"]))
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        private async Task SetAccessTokenCookie(string accessToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["TokenSettings:AccessTokenExpiryMinutes"]))
            };

            Response.Cookies.Append("accessToken", accessToken, cookieOptions);
        }
    }
}
