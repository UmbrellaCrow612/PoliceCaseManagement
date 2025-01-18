using Authorization.Core;
using Identity.API.Annotations;
using Identity.API.DTOs;
using Identity.API.Helpers;
using Identity.API.Mappings;
using Identity.API.Services.Interfaces;
using Identity.API.Settings;
using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Utils.DTOs;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController(
        UserManager<ApplicationUser> userManager, IOptions<JwtBearerOptions> JWTOptions,
        ITokenStore tokenStore, IPasswordResetAttemptStore passwordResetAttemptStore,
        IEmailVerificationAttemptStore emailVerificationAttemptStore, IUserDeviceStore userDeviceStore,
        IUserDeviceChallengeAttemptStore userDeviceChallengeAttemptStore, IDeviceIdentification deviceIdentification, IPhoneConfirmationAttemptStore phoneConfirmationAttemptStore,
        DeviceManager deviceManager, IAuthService authService
        ) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly JwtBearerOptions _JWTOptions = JWTOptions.Value;
        private readonly ITokenStore _tokenStore = tokenStore;
        private readonly IPasswordResetAttemptStore _passwordResetAttemptStore = passwordResetAttemptStore;
        private readonly IEmailVerificationAttemptStore _emailVerificationAttemptStore = emailVerificationAttemptStore;
        private readonly IUserDeviceStore _userDeviceStore = userDeviceStore;
        private readonly IUserDeviceChallengeAttemptStore _userDeviceChallengeAttemptStore = userDeviceChallengeAttemptStore;
        private readonly IDeviceIdentification _deviceIdentification = deviceIdentification;
        private readonly IPhoneConfirmationAttemptStore _phoneConfirmationAttemptStore = phoneConfirmationAttemptStore;
        private readonly DeviceManager _deviceManager = deviceManager;
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

        [RequireDeviceInformation]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var info = ComposeDeviceInfo();

            var res = await _authService.LoginAsync(dto.Email, dto.Password, info);
            if (!res.Succeeded) return Unauthorized(res.Errors);

            return Ok(new { res.LoginAttemptId });
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("validate-two-factor-sms")]
        public async Task<ActionResult> ValidateTwoFactorAuthentication(ValidateTwoFactorSmsAttemptDto dto)
        {
            var info = ComposeDeviceInfo();

            var result = await _authService.ValidateTwoFactorSmsCodeAsync(dto.LoginAttemptId, dto.Code, info);
            if (!result.Succeeded) return Unauthorized(result.Errors);

            Response.Cookies.Append(CookieNamesConstant.JWT, result.Tokens.JwtBearerToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_JWTOptions.ExpiresInMinutes)
            });


            Response.Cookies.Append(CookieNamesConstant.REFRESH_TOKEN, result.Tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_JWTOptions.RefreshTokenExpiriesInMinutes)
            });

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
            var info = ComposeDeviceInfo();

            var res = await _authService.ValidateTwoFactorEmailCodeAsync(dto.LoginAttemptId, dto.Code, info);
            if (!res.Succeeded) return Unauthorized(res.Errors);

            Response.Cookies.Append(CookieNamesConstant.JWT, res.Tokens.JwtBearerToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_JWTOptions.ExpiresInMinutes)
            });


            Response.Cookies.Append(CookieNamesConstant.REFRESH_TOKEN, res.Tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_JWTOptions.RefreshTokenExpiriesInMinutes)
            });


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

            var info = ComposeDeviceInfo();
            var result = await _authService.RefreshTokens(userId, tokenId, dto.RefreshToken, info);
            if (!result.Succeeded) return Unauthorized(result.Errors);

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

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return Unauthorized();

            await _tokenStore.RevokeAllUserTokensAsync(user.Id);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequestDto.Email);
            if (user is null) return Ok(); // We don't reveal if a user exists

            var trueCode = Guid.NewGuid().ToString();

            PasswordResetAttempt passwordResetAttempt = new()
            {
                Code = trueCode,
                UserId = user.Id,
            };

            (bool canMakeAttempt, ICollection<ErrorDetail> errors) = await _passwordResetAttemptStore.AddAttempt(passwordResetAttempt);

            if (!canMakeAttempt)
            {
                return BadRequest(errors);
            }

            // Send Email link with true code

            return Ok(new { trueCode });
        }

        [AllowAnonymous]
        [HttpPost("confirm-password-reset/{code}")]
        public async Task<ActionResult> ConfirmResetPassword(string code, [FromBody] ConfirmPasswordResetRequestDto confirmPasswordResetRequestDto)
        {
            (bool isValid, PasswordResetAttempt? attempt) = await _passwordResetAttemptStore.ValidateAttempt(code);
            if (!isValid) return Unauthorized(); // Either session expired or code is wrong for latest attempt or already used

            if (attempt is null) return Unauthorized(); // attempt is either not found or time elapsed and is no longer valid

            var user = await _userManager.FindByIdAsync(attempt.UserId);
            if (user is null) return BadRequest();

            attempt.MarkUsed();

            _passwordResetAttemptStore.SetToUpdateAttempt(attempt);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, confirmPasswordResetRequestDto.NewPassword);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, changePasswordRequestDto.Password, changePasswordRequestDto.NewPassword);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("validate-confirmation-email")]
        public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
        {
            (bool isValid, EmailVerificationAttempt? attempt, ApplicationUser? user, ICollection<string> errors) = await _emailVerificationAttemptStore
                .ValidateAttemptAsync(confirmEmailDto.Email, confirmEmailDto.Code);

            if (!isValid) return BadRequest(errors);

            if (attempt is null || user is null) return Unauthorized();

            user.EmailConfirmed = true;

            attempt.MarkUsed();

            _emailVerificationAttemptStore.SetToUpdateAttempt(attempt);

            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return BadRequest(res.Errors);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("resend-confirmation-email")]
        public async Task<ActionResult> ResendConfirmEmail([FromBody] ResendConfirmationEmailDto resendConfirmationEmailDto)
        {
            var user = await _userManager.FindByEmailAsync(resendConfirmationEmailDto.Email);
            if (user is null) return Unauthorized();

            if (user.EmailConfirmed) return BadRequest(new ErrorDetail
            {
                Field = "Email confirmation.",
                Reason = "User email already comfirmed."
            });

            var attempt = new EmailVerificationAttempt()
            {
                Email = resendConfirmationEmailDto.Email,
                Code = Guid.NewGuid().ToString(),
                UserId = user.Id
            };

            (bool canMakeAttempt, ICollection<ErrorDetail> errors) = await _emailVerificationAttemptStore.AddAttemptAsync(attempt);
            if (!canMakeAttempt) return BadRequest(errors);

            // send code in email

            return Ok(attempt.Code);
        }


        [AllowAnonymous]
        [HttpPost("validate-user-device-challenge")]
        public async Task<ActionResult> Challenge([FromBody] UserDeviceChallengeDto challengeDto)
        {
            var user = await _userManager.FindByEmailAsync(challengeDto.Email);
            if (user is null) return Ok("User dose not exist - would not show in prod");

            (bool isValid, UserDeviceChallengeAttempt? attempt) = await _userDeviceChallengeAttemptStore.ValidateAttemptAsync(challengeDto.Email, challengeDto.Code);
            if (!isValid || attempt is null) return BadRequest("Attempt is invalid or attempt not found.");

            attempt.MarkUsed();
            _userDeviceChallengeAttemptStore.SetToUpdateAttempt(attempt);

            var info = ComposeDeviceInfo();
            var device = await _deviceManager.GetRequestingDevice(user.Id, info.DeviceFingerPrint, info.UserAgent);
            if (device is null) return NotFound("Device not found.");

            device.IsTrusted = true;

            await _userDeviceStore.UpdateAsync(device);

            return Ok();
        }

        [RequireDeviceInformation]
        [AllowAnonymous]
        [HttpPost("resend-user-device-challenge")]
        public async Task<ActionResult> ReSendChallenge([FromBody] ReSendUserDeviceChallengeDto challengeDto)
        {
            var user = await _userManager.FindByEmailAsync(challengeDto.Email);
            if (user is null) return Ok();

            var code = Guid.NewGuid().ToString();

            var deviceId = _deviceIdentification.GenerateDeviceId(user.Id, Request.Headers.UserAgent!, Request.HttpContext.Request.Headers[CustomHeaderOptions.XDeviceFingerprint].FirstOrDefault()!);

            var info = ComposeDeviceInfo();
            var device = await _deviceManager.GetRequestingDevice(user.Id, info.DeviceFingerPrint, info.UserAgent);
            if (device is not null && device.IsTrusted is true) return BadRequest();

            device ??= new UserDevice
            {
                Id = deviceId,
                DeviceName = Request.Headers.UserAgent.ToString(),
                UserId = user.Id,
            };

            await _userDeviceStore.SetUserDevice(user, device);

            var attempt = new UserDeviceChallengeAttempt
            {
                Code = code,
                Email = challengeDto.Email,
                UserDeviceId = deviceId,
                UserId = user.Id,
            };

            (bool canMakeAttempt, ICollection<string> Errors) = await _userDeviceChallengeAttemptStore.AddAttempt(attempt);
            if (!canMakeAttempt) return BadRequest(Errors);

            return Ok(new { code });
        }

        [AllowAnonymous]
        [HttpPost("validate-phone-confirmation")]
        public async Task<ActionResult> PhoneConfirmation([FromBody] PhoneConfirmationDto phoneConfirmationDto)
        {
            (bool isValid, PhoneConfirmationAttempt? attempt, ICollection<ErrorDetail> errors) =
                await _phoneConfirmationAttemptStore.ValidateAttempt(phoneConfirmationDto.PhoneNumber, phoneConfirmationDto.Code);

            if (!isValid) return BadRequest(errors);

            if (attempt is null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(attempt.UserId);
            if (user is null) return Unauthorized();

            attempt.MarkUsed();
            _phoneConfirmationAttemptStore.SetToUpdate(attempt);

            user.PhoneNumberConfirmed = true;

            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return BadRequest(res.Errors);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("resend-phone-confirmation")]
        public async Task<ActionResult> ReSendPhoneConfirmation([FromBody] ReSendPhoneConfirmationDto reSendPhoneConfirmation)
        {
            var user = await _userManager.FindByEmailAsync(reSendPhoneConfirmation.Email);
            if (user is null) return NotFound("User not found.");

            if (user.PhoneNumberConfirmed) return BadRequest(new ErrorDetail
            { Field = "Phone Confirmation", Reason = "Users phone number already confirmed." });

            var attempt = new PhoneConfirmationAttempt
            {
                Code = Guid.NewGuid().ToString(),
                PhoneNumber = user.PhoneNumber!,
                UserId = user.Id,
            };

            (bool canMakeAttempt, ICollection<ErrorDetail> errors) = await _phoneConfirmationAttemptStore.AddAttempt(attempt);
            if (!canMakeAttempt) return BadRequest(errors);

            // send code in sms message

            return Ok(attempt.Code);
        }
    }
}
