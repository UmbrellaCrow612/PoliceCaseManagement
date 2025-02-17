using Authorization.Core;
using Identity.API.Annotations;
using Identity.API.DTOs;
using Identity.API.Mappings;
using Identity.API.Services.Interfaces;
using Identity.API.Settings;
using Identity.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Identity.API.Extensions;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController(
        UserManager<ApplicationUser> userManager, IOptions<JwtBearerOptions> JWTOptions,
        IAuthService authService
        ) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly JwtBearerOptions _JWTOptions = JWTOptions.Value;
        private readonly UserMapping userMapping = new();
        private readonly IAuthService _authService = authService;

        private DeviceInfo ComposeDeviceInfo()
        {
            return new DeviceInfo
            {
                DeviceFingerPrint = Request.Headers[CustomHeaderOptions.XDeviceFingerprint].FirstOrDefault()!,
                IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
                                ?? Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                ?? "Unknown",
                UserAgent = Request.Headers.UserAgent.ToString()
            };
        }

        [Authorize]
        [HttpPost("turn-on-totp")]
        public async Task<ActionResult> TurnOnTOTP()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return Unauthorized();

            user.TimeBasedOneTimePassCodeEnabled = true;
            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return Unauthorized(res.Errors);

            return Ok();
        }

        [RequireDeviceInformation]
        [Authorize]
        [HttpPost("setup-totp")]
        public async Task<ActionResult> SetUpTOTP()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var res = await _authService.SetUpTOTP(userId, ComposeDeviceInfo());
            if (!res.Succeeded) return Unauthorized(res.Errors);

            var file = File(res.TotpSecretQrCodeBytes, "image/png", "totp-qrcode-image");

            return file;
        }

        [AllowAnonymous]
        [RequireDeviceInformation]
        [HttpPost("validate-totp")]
        public async Task<ActionResult> ValidateTOTP([FromBody] ValidateTOTPDto dto)
        {
            var res = await _authService.ValidateTOTP(dto.Code,dto.LoginAttemptId, ComposeDeviceInfo());
            if (!res.Succeeded) return Unauthorized(res.Errors);

            this.SetAuthCookies(res.Tokens, _JWTOptions);

            return Ok(new { res.Tokens });
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("validate-otp")]
        public async Task<ActionResult> ValidateOtp([FromBody] ValidateOtpDto dto)
        {
            var res = await _authService.ValidateOTP(dto.OTPMethod, dto.OTPCreds, dto.Code, ComposeDeviceInfo());
            if (!res.Succeeded) return Unauthorized(res.Errors);

            this.SetAuthCookies(res.Tokens, _JWTOptions);

            return Ok(new { res.Tokens });
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("send-otp")]
        public async Task<ActionResult> SendOtp([FromBody] SendOtpDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.OTPCreds.Email) && string.IsNullOrWhiteSpace(dto.OTPCreds.PhoneNumber)) return BadRequest();

            var res = await _authService.SendOTP(dto.OTPMethod, dto.OTPCreds, ComposeDeviceInfo());
            if (!res.Succeeded) return Unauthorized(res.Errors);

            var file = File(res.QrCodeBytes, "image/png", "otp-qrcode-image");

            return file;
        }

        [Authorize]
        [HttpPost("turn-on-otp")]
        public async Task<ActionResult> TurnOnOTP()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return Unauthorized();

            user.OTPAuthEnabled = true;
            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return Unauthorized(res.Errors);

            return Ok();
        }

        [HttpPost("turn-on-magic-link")]
        [Authorize]
        public async Task<ActionResult> TunrnOnMagicLinkAuth()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return Unauthorized();

            user.MagicLinkAuthEnabled = true;
            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return Unauthorized(res.Errors);

            return Ok();
        }

        [AllowAnonymous]
        [RequireDeviceInformation]
        [HttpPost("send-magic-link")]
        public async Task<ActionResult> SendMagicLink([FromBody] string email)
        {
            var res = await _authService.SendMagicLink(email, ComposeDeviceInfo());
            if (!res.Succeeded) return Unauthorized(res.Errors);

            return Ok();
        }

        [AllowAnonymous]
        [RequireDeviceInformation]
        [HttpPost("validate-magic-link")]
        public async Task<ActionResult> ValidateMagicLink([FromBody] string code)
        {
            var res = await _authService.ValidateMagicLink(code, ComposeDeviceInfo());
            if (!res.Succeeded) return Unauthorized(res.Errors);

            this.SetAuthCookies(res.Tokens, _JWTOptions);

            return Ok(new { res.Tokens.JwtBearerToken, res.Tokens.RefreshToken });
        }


        [RequireDeviceInformation]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var res = await _authService.LoginAsync(dto.Email, dto.Password, ComposeDeviceInfo());
            if (!res.Succeeded) return Unauthorized(res.Errors);

            return Ok(new { res.LoginAttemptId });
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("validate-two-factor-sms")]
        public async Task<ActionResult> ValidateTwoFactorAuthentication(ValidateTwoFactorSmsAttemptDto dto)
        {
            var result = await _authService.ValidateTwoFactorSmsCodeAsync(dto.LoginAttemptId, dto.Code, ComposeDeviceInfo());
            if (!result.Succeeded) return Unauthorized(result.Errors);

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
            var res = await _authService.ValidateTwoFactorEmailCodeAsync(dto.LoginAttemptId, dto.Code, ComposeDeviceInfo());
            if (!res.Succeeded) return Unauthorized(res.Errors);
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

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var userToCreate = userMapping.Create(registerRequestDto);
            var phoneNumberTaken = await _userManager.Users.AnyAsync(x => x.PhoneNumber == registerRequestDto.PhoneNumber);
            if (phoneNumberTaken) return BadRequest(new { message = "Phone number taken" });

            var result = await _userManager.CreateAsync(userToCreate, registerRequestDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var dto = userMapping.ToDto(userToCreate);

            return Ok(dto);
        }

        [RequireDeviceInformation]
        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tokenId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            if (string.IsNullOrEmpty(tokenId))
            {
                return Unauthorized("Jti ID not found in token.");
            }

            var result = await _authService.RefreshTokens(userId, tokenId, dto.RefreshToken, ComposeDeviceInfo());
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

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var res = await _authService.LogoutAsync(userId);
            if (!res.Succeeded) return Unauthorized(res.Errors);

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
            if (!res.Succeeded) return Unauthorized(res.Errors);

            return Ok();
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("send-user-device-challenge")]
        public async Task<ActionResult> ReSendChallenge([FromBody] ReSendUserDeviceChallengeDto dto)
        {
            var res = await _authService.SendUserDeviceChallenge(dto.Email, ComposeDeviceInfo());
            if (!res.Succeeded) return Unauthorized(res.Errors);

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
