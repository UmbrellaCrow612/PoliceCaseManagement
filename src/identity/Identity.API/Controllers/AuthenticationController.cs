using Identity.API.Annotations;
using Identity.API.DTOs;
using Identity.API.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Identity.API.Extensions;
using Identity.Core.Services;
using Authorization;

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
        private readonly UserMapping userMapping = new();
        private readonly IAuthService _authService = authService;

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var result = await _authService.ChangePassword(this.ComposeDeviceInfo(), dto.Email, dto.Password, dto.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok();
        }

        [Authorize]
        [HttpPost("turn-on-totp")]
        public async Task<ActionResult> TurnOnTOTP()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId)) return BadRequest();
            var result = await _authService.TurnOnTOTP(userId);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok();
        }

        [RequireDeviceInformation]
        [Authorize]
        [HttpPost("setup-totp")]
        public async Task<ActionResult> SetUpTOTP()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId)) return BadRequest();

            var res = await _authService.SetUpTOTP(userId, this.ComposeDeviceInfo());
            if (!res.Succeeded) return BadRequest(res.Errors);

            var file = File(res.TotpSecretQrCodeBytes, "image/png", "totp-qrcode-image");

            return file;
        }

        [AllowAnonymous]
        [RequireDeviceInformation]
        [HttpPost("validate-totp")]
        public async Task<ActionResult> ValidateTOTP([FromBody] ValidateTOTPDto dto)
        {
            var res = await _authService.ValidateTOTP(dto.Code,dto.LoginAttemptId, this.ComposeDeviceInfo());
            if (!res.Succeeded) return BadRequest(res.Errors);

            this.SetAuthCookies(res.Tokens, _JWTOptions);

            return Ok(new { res.Tokens });
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("validate-otp")]
        public async Task<ActionResult> ValidateOtp([FromBody] ValidateOtpDto dto)
        {
            var res = await _authService.ValidateOTP(dto.OTPMethod, dto.OTPCreds, dto.Code, this.ComposeDeviceInfo());
            if (!res.Succeeded) return BadRequest(res.Errors);

            this.SetAuthCookies(res.Tokens, _JWTOptions);

            return Ok(new { res.Tokens });
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("send-otp")]
        public async Task<ActionResult> SendOtp([FromBody] SendOtpDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.OTPCreds.Email) && string.IsNullOrWhiteSpace(dto.OTPCreds.PhoneNumber)) return BadRequest();

            var res = await _authService.SendOTP(dto.OTPMethod, dto.OTPCreds, this.ComposeDeviceInfo());
            if (!res.Succeeded) return BadRequest(res.Errors);

            var file = File(res.QrCodeBytes, "image/png", "otp-qrcode-image");

            return file;
        }

        [Authorize]
        [HttpPost("turn-on-otp")]
        public async Task<ActionResult> TurnOnOTP()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest();

            var result = await _authService.TurnOnOTP(userId);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok();
        }

        [Authorize]
        [HttpPost("turn-on-magic-link")]
        public async Task<ActionResult> TunrnOnMagicLinkAuth()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest();

            var result = await _authService.TurnOnMagicLink(userId);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok();
        }

        [AllowAnonymous]
        [RequireDeviceInformation]
        [HttpPost("send-magic-link")]
        public async Task<ActionResult> SendMagicLink([FromBody] string email)
        {
            var res = await _authService.SendMagicLink(email, this.ComposeDeviceInfo());
            if (!res.Succeeded) return BadRequest(res.Errors);

            return Ok();
        }

        [AllowAnonymous]
        [RequireDeviceInformation]
        [HttpPost("validate-magic-link")]
        public async Task<ActionResult> ValidateMagicLink([FromBody] string code)
        {
            var res = await _authService.ValidateMagicLink(code, this.ComposeDeviceInfo());
            if (!res.Succeeded) return BadRequest(res.Errors);

            this.SetAuthCookies(res.Tokens, _JWTOptions);

            return Ok(new { res.Tokens.JwtBearerToken, res.Tokens.RefreshToken });
        }


        [RequireDeviceInformation]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var res = await _authService.LoginAsync(dto.Email, dto.Password, this.ComposeDeviceInfo());
            if (!res.Succeeded) return BadRequest(res.Errors);

            return Ok(new { res.LoginAttemptId });
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("validate-two-factor-sms")]
        public async Task<ActionResult> ValidateTwoFactorAuthentication(ValidateTwoFactorSmsAttemptDto dto)
        {
            var result = await _authService.ValidateTwoFactorSmsCodeAsync(dto.LoginAttemptId, dto.Code, this.ComposeDeviceInfo());
            if (!result.Succeeded) return BadRequest(result.Errors);

            this.SetAuthCookies(result.Tokens, _JWTOptions);

            return Ok(new { result.Tokens.JwtBearerToken, result.Tokens.RefreshToken });
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("send-two-factor-sms")]
        public async Task<ActionResult> ReSendTwoFactorAuthentication(ReSendTwoFactorCode dto)
        {
            var res = await _authService.SendTwoFactorSmsVerificationCodeAsync(dto.LoginAttemptId);
            if (!res.Succeeded) return BadRequest(res.Errors);

            return Ok();
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("validate-two-factor-email")]
        public async Task<ActionResult> ValidateTwoFactorEmailAuth([FromBody] ValidateTwoFactorEmailAttemptDto dto)
        {
            var res = await _authService.ValidateTwoFactorEmailCodeAsync(dto.LoginAttemptId, dto.Code, this.ComposeDeviceInfo());
            if (!res.Succeeded) return BadRequest(res.Errors);
            this.SetAuthCookies(res.Tokens, _JWTOptions);

            return Ok(new { res.Tokens.JwtBearerToken, res.Tokens.RefreshToken });
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("send-two-factor-email")]
        public async Task<ActionResult> ReSendTwoFactorEmailAuth([FromBody] ReSendTwoFactorEmailAttemptDto dto)
        {
            var res = await _authService.SendTwoFactorEmailVerificationCodeAsync(dto.LoginAttemptId);
            if (!res.Succeeded) return BadRequest(res.Errors);

            return Ok();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            var userToCreate = userMapping.Create(dto);

            var result = await _authService.RegisterUserAsync(userToCreate, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var returnDto = userMapping.ToDto(userToCreate);

            return Ok(returnDto);
        }

        [Authorize]
        [RequireDeviceInformation]
        [HttpGet("refresh-token")]
        public async Task<ActionResult> RefreshToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tokenId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            Request.Cookies.TryGetValue(CookieNamesConstant.REFRESH_TOKEN, out string? refreshToken);

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return Unauthorized("Refresh cookie missing"); // we send 401 here as if it cannoit refresh it means they should be logged out 
            }

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            if (string.IsNullOrEmpty(tokenId))
            {
                return Unauthorized("Jti ID not found in token.");
            }

            var result = await _authService.RefreshTokensAsync(userId, tokenId, refreshToken, this.ComposeDeviceInfo());
            if (!result.Succeeded) return Unauthorized(result.Errors);

            /// we are not using <see cref="ControllerExtensions.SetAuthCookies(ControllerBase, Tokens, JwtBearerOptions)"/> becuase we only send back a new jwt
            Response.Cookies.Append(CookieNamesConstant.JWT, result.Tokens.JwtBearerToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_JWTOptions.ExpiresInMinutes)
            });

            return Ok(new { result.Tokens.JwtBearerToken });
        }

        /// <summary>
        /// Done thiwa way as front end calls this when logging out and listens to 401 and calls logout etc
        /// meaning if we send 401 it calls recusive calls
        /// </summary>
        /// <returns></returns>
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

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            var res = await _authService.SendResetPassword(dto.Email);
            if (!res.Succeeded) return BadRequest();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset-password/{code}")]
        public async Task<ActionResult> ConfirmResetPassword(string code, [FromBody] ConfirmPasswordResetRequestDto dto)
        {
            var res = await _authService.ValidateResetPassword(code, dto.NewPassword);
            if (!res.Succeeded) return BadRequest();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailDto dto)
        {
            var res = await _authService.ConfirmEmail(dto.Email, dto.Code);
            if (!res.Succeeded) return BadRequest(res.Errors);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("send-confirmation-email")]
        public async Task<ActionResult> ResendConfirmEmail([FromBody] ResendConfirmationEmailDto dto)
        {
            var res = await _authService.SendConfirmationEmail(dto.Email);
            if (!res.Succeeded) return BadRequest(res.Errors);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("validate-user-device-challenge")]
        public async Task<ActionResult> Challenge([FromBody] UserDeviceChallengeDto dto)
        {
            var res = await _authService.ValidateUserDeviceChallenge(dto.Email, dto.Code);
            if (!res.Succeeded) return BadRequest(res.Errors);

            return Ok();
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("send-user-device-challenge")]
        public async Task<ActionResult> ReSendChallenge([FromBody] ReSendUserDeviceChallengeDto dto)
        {
            var res = await _authService.SendUserDeviceChallenge(dto.Email, this.ComposeDeviceInfo());
            if (!res.Succeeded) return BadRequest(res.Errors);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("validate-phone-confirmation")]
        public async Task<ActionResult> PhoneConfirmation([FromBody] PhoneConfirmationDto dto)
        {
            var res = await _authService.ConfirmPhoneNumber(dto.PhoneNumber, dto.Code);
            if (!res.Succeeded) return BadRequest(res.Errors);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("send-phone-confirmation")]
        public async Task<ActionResult> ReSendPhoneConfirmation([FromBody] SendPhoneConfirmationDto dto)
        {
            var res = await _authService.SendPhoneConfirmation(dto.PhoneNumber);
            if (!res.Succeeded) return BadRequest(res.Errors);

            return Ok();
        }
    }
}
