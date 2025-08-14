using System.Security.Claims;
using Authorization;
using Identity.API.Annotations;
using Identity.API.DTOs;
using Identity.API.Extensions;
using Identity.Application.Helpers;
using Identity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController(
        IOptions<JwtBearerOptions> JWTOptions,
        IAuthService authService
        ) : ControllerBase
    {
        private readonly JwtBearerOptions _JWTOptions = JWTOptions.Value;
        private readonly IAuthService _authService = authService;

        [RequireDeviceInformation]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResult>> Login([FromBody] LoginDto dto)
        {
            var res = await _authService.LoginAsync(dto.Email, dto.Password, this.ComposeDeviceInfo());
            if (!res.Succeeded) return BadRequest(res);

            return Ok(new { res.LoginId });
        }


        [RequireDeviceInformation]
        [HttpGet("refresh-token")]
        public async Task<ActionResult> RefreshToken()
        {
            Request.Cookies.TryGetValue(AuthCookieNamesConstant.REFRESH_TOKEN, out string? refreshToken);

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Refresh token missing");
            }

            var result = await _authService.RefreshTokensAsync(refreshToken, this.ComposeDeviceInfo());
            if (!result.Succeeded)
            {
                this.RemoveAuthCookies();
                return Unauthorized(result.Errors);
            }

            this.SetAuthCookies(result.Tokens, _JWTOptions);

            return Ok(new { result.Tokens.JwtBearerToken });
        }

      
        [AllowAnonymous]
        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found in token.");
            }

            var res = await _authService.LogoutAsync(userId);
            if (!res.Succeeded) return BadRequest(res.Errors);

            this.RemoveAuthCookies();

            return Ok();
        }
    }
}
