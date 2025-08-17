using Identity.API.Annotations;
using Identity.API.DTOs;
using Identity.API.Extensions;
using Identity.Application.Helpers;
using Identity.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Identity.API.Controllers
{
    /// <summary>
    /// Contains all Multi factor auth logic
    /// </summary>
    [ApiController]
    [Route("mfa")]
    public class MfaController(IMfaService mfaService, IOptions<JwtBearerOptions> jwtBearerOptions) : ControllerBase
    {
        private readonly IMfaService _mfaService = mfaService;
        private readonly JwtBearerOptions _jwtBearerOptions = jwtBearerOptions.Value;


        [RequireDeviceInformation]
        [HttpPost("sms")]
        public async Task<IActionResult> SendMfaSms([FromBody] MfaSmsDto dto)
        {
            var result = await _mfaService.SendMfaSmsAsync(dto.LoginId, this.ComposeDeviceInfo());
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [RequireDeviceInformation]
        [HttpPost("verify/sms")]
        public async Task<IActionResult> VerifyMfaSms([FromBody] VerifyMfaSmsDto dto)
        {
            var result = await _mfaService.VerifyMfaSmsAsync(dto.LoginId, dto.Code, this.ComposeDeviceInfo());
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            this.SetAuthCookies(result.Tokens, _jwtBearerOptions);

            return Ok(new JwtResponseDto { Jwt = result.Tokens.JwtBearerToken });
        }

        [RequireDeviceInformation]
        [HttpPost("verify/totp")]
        public async Task<IActionResult> VerifyTotp([FromBody] VerifyMfaTotpDto dto)
        {
            var result = await _mfaService.VerifyTotpAsync(dto.LoginId, dto.Code, this.ComposeDeviceInfo());
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            this.SetAuthCookies(result.Tokens, _jwtBearerOptions);

            return Ok(new JwtResponseDto { Jwt = result.Tokens.JwtBearerToken });
        }
    }
}
