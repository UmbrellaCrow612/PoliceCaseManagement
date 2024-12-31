using Identity.API.DTOs;
using Identity.API.Helpers;
using Identity.API.Settings;
using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Identity.Infrastructure.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController(
        JwtHelper jwtHelper, UserManager<ApplicationUser> userManager, IOptions<JWTOptions> JWTOptions,
        StringEncryptionHelper stringEncryptionHelper, ITokenStore tokenStore, IPasswordResetAttemptStore passwordResetAttemptStore, 
        ILogger<AuthenticationController> logger, ILoginAttemptStore loginAttemptStore,
        IEmailVerificationAttemptStore emailVerificationAttemptStore, IUserDeviceStore userDeviceStore,
        IUserDeviceChallengeAttemptStore userDeviceChallengeAttemptStore, IDeviceIdentification deviceIdentification, IPhoneConfirmationAttemptStore phoneConfirmationAttemptStore,
        ITwoFactorSmsAttemptStore twoFactorSmsAttemptStore, IOptions<TimeWindows> timeWindows, DeviceManager deviceManager, ITwoFactorEmailAttemptStore twoFactorEmailAttemptStore
        ) : ControllerBase
    {
        private readonly JwtHelper _jwtHelper = jwtHelper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly JWTOptions _JWTOptions = JWTOptions.Value;
        private readonly StringEncryptionHelper _stringEncryptionHelper = stringEncryptionHelper;
        private readonly ITokenStore _tokenStore = tokenStore;
        private readonly IPasswordResetAttemptStore _passwordResetAttemptStore = passwordResetAttemptStore;
        private readonly ILogger<AuthenticationController> _logger = logger;
        private readonly ILoginAttemptStore _loginAttemptStore = loginAttemptStore;
        private readonly IEmailVerificationAttemptStore _emailVerificationAttemptStore = emailVerificationAttemptStore;
        private readonly IUserDeviceStore _userDeviceStore = userDeviceStore;
        private readonly IUserDeviceChallengeAttemptStore _userDeviceChallengeAttemptStore = userDeviceChallengeAttemptStore;
        private readonly IDeviceIdentification _deviceIdentification = deviceIdentification;
        private readonly IPhoneConfirmationAttemptStore _phoneConfirmationAttemptStore = phoneConfirmationAttemptStore;
        private readonly ITwoFactorSmsAttemptStore _twoFactorSmsAttemptStore = twoFactorSmsAttemptStore;
        private readonly TimeWindows _timeWindows = timeWindows.Value;
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly ITwoFactorEmailAttemptStore _twoFactorEmailAttemptStore = twoFactorEmailAttemptStore;

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
                                ?? Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                ?? "Unknown";

            var (isValid, errors) = _deviceManager.VerifyRequestHasRequiredProperties(Request);
            if (!isValid) return BadRequest(errors);

            ApplicationUser? user;
            if (!string.IsNullOrWhiteSpace(dto.UserName))
            {
                user = await _userManager.FindByNameAsync(dto.UserName);
                if (user is null) return Unauthorized("Incorrect credentials");
            }
            else
            {
                user = await _userManager.FindByEmailAsync(dto.Email ?? string.Empty);
                if (user is null) return Unauthorized("Incorrect credentials");
            }

            if (user is null) return Unauthorized();

            string userAgent = Request.Headers.UserAgent.ToString();
            LoginAttempt loginAttempt = new()
            {
                IpAddress = ipAddress,
                UserAgent = userAgent,
                UserId = user.Id,
            };
           
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordCorrect)
            {
                loginAttempt.FailureReason = "User credentials";
                await _loginAttemptStore.StoreLoginAttempt(loginAttempt);

                return Unauthorized("Incorrect credentials");
            }

            if (user.LockoutEnabled is true && user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                loginAttempt.FailureReason = "User account locked.";
                await _loginAttemptStore.StoreLoginAttempt(loginAttempt);

                return Unauthorized("User account locked.");
            }

            if (!user.EmailConfirmed)
            {
                loginAttempt.FailureReason = "Email not confirmed.";
                await _loginAttemptStore.StoreLoginAttempt(loginAttempt);

                return StatusCode(403, new
                {
                    redirectUrl = "/emailConfirm",
                    message = "Email needs confirmation"
                });
            }

            if (!user.PhoneNumberConfirmed)
            {
                loginAttempt.FailureReason = "Phone not confirmed.";
                await _loginAttemptStore.StoreLoginAttempt(loginAttempt);

                return StatusCode(403, new
                {
                    redirectUrl = "/phoneConfirm",
                    message = "Phone needs confirmation"
                });
            }

            var device = await _deviceManager.GetRequestingDevice(user.Id, Request);
            if (device is null)
            {
                loginAttempt.FailureReason = "New Device being used.";
                await _loginAttemptStore.StoreLoginAttempt(loginAttempt);

                return StatusCode(403, new
                {
                    redirectUrl = "/deviceConfirm",
                    message = "Device needs confirmation"
                });
            }

            if (!device.IsTrusted)
            {
                loginAttempt.FailureReason = "Untrusted Device being used.";
                await _loginAttemptStore.StoreLoginAttempt(loginAttempt);

                return StatusCode(403, new
                {
                    redirectUrl = "/deviceConfirm?untrusted-device-used=true",
                    message = "Device needs confirmation"
                });
            }

            loginAttempt.Status = LoginStatus.TwoFactorAuthenticationReached;
            await _loginAttemptStore.StoreLoginAttempt(loginAttempt);

            // send a redirect to /tfa?loginAttempt=id - they will choose a TFA method for this attempt

            return Ok(new {loginAttempt.Id});
        }

        [AllowAnonymous]
        [HttpPost("validate-two-factor-sms-authentication")]
        public async Task<ActionResult> ValidateTwoFactorAuthentication(ValidateTwoFactorSmsAttemptDto dto)
        {
            var devRes = _deviceManager.VerifyRequestHasRequiredProperties(Request);
            if (!devRes.isValid) return BadRequest(devRes.errors);

            var loginAttempt = await _loginAttemptStore.GetLoginAttemptById(dto.LoginAttemptId);
            if (loginAttempt is null || !loginAttempt.IsValid(_timeWindows.LoginLifetime)) return Unauthorized(new ErrorDetail
            {
                Field = "Two factor sms authentication.",
                Reason = "Login attempt not found or is invalid."
            });

            var (isValid, errors) = await _twoFactorSmsAttemptStore.ValidateAttempt(loginAttempt.Id, dto.Code);
            if (!isValid) return Unauthorized(errors);

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if (user is null) return NotFound(new ErrorDetail { Field = "Two factor sms authentication", Reason = "User associated with login attempt not found." });

            var device = await _deviceManager.GetRequestingDevice(user.Id, Request);
            if (device is null || !device.Trusted())
            {
                return StatusCode(403, new
                {
                    redirectUrl = "/deviceConfirm",
                    message = "Device needs confirmation"
                });
            }

            var roles = await _userManager.GetRolesAsync(user);

            (string accessToken, string tokenId) = _jwtHelper.GenerateToken(user, roles);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            Token token = new()
            {
                Id = tokenId,
                RefreshToken = _stringEncryptionHelper.Hash(refreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(_JWTOptions.RefreshTokenExpiriesInMinutes),
                UserId = user.Id,
                UserDeviceId = device.Id
            };

            await _tokenStore.SetToken(token);

            user.LastLoginDeviceId = device.Id;

            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return BadRequest(res.Errors);

            Response.Cookies.Append(CookieNamesConstant.JWT, accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_JWTOptions.ExpiresInMinutes) 
            });

           
            Response.Cookies.Append(CookieNamesConstant.REFRESH_TOKEN, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_JWTOptions.RefreshTokenExpiriesInMinutes) 
            });

            return Ok(new { accessToken, refreshToken });
        }

        [AllowAnonymous]
        [HttpPost("resend-two-factor-sms-authentication")]
        public async Task<ActionResult> ReSendTwoFactorAuthentication(ReSendTwoFactorCode reSendTwoFactorCode)
        {
            var devRes = _deviceManager.VerifyRequestHasRequiredProperties(Request);
            if (!devRes.isValid) return BadRequest(devRes.errors);

            var loginAttempt = await _loginAttemptStore.GetLoginAttemptById(reSendTwoFactorCode.LoginAttemptId);
            if (loginAttempt is null || !loginAttempt.IsValid(_timeWindows.LoginLifetime)) return Unauthorized(new ErrorDetail
            {
                Field = "Two factor sms authentication.",
                Reason = "Login attempt not found or is invalid."
            });

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if (user is null) return Unauthorized(new ErrorDetail
            {
                Field = "Two factor sms authentication",
                Reason = "Login attempt User dose not exist."
            });

            var device = await _deviceManager.GetRequestingDevice(user.Id, Request);
            if (device is null || !device.Trusted()) { return StatusCode(403, new { redirectUrl = "/deviceConfirm", message = "Device needs confirmation"});}

            TwoFactorSmsAttempt attempt = new()
            {
                Code = Guid.NewGuid().ToString(),
                LoginAttemptId = loginAttempt.Id,
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber!
            };

            var (canMakeAttempt, errors) = await _twoFactorSmsAttemptStore.AddAttempt(attempt);
            if (!canMakeAttempt) return BadRequest(errors);

            // send sms for this login attempt

            return Ok(attempt.Code);
        }


        [AllowAnonymous]
        [HttpPost("validate-two-factor-email-authentication")]
        public async Task<ActionResult> ValidateTwoFactorEmailAuth([FromBody] ValidateTwoFactorEmailAttemptDto dto)
        {
            var devRes = _deviceManager.VerifyRequestHasRequiredProperties(Request);
            if (!devRes.isValid) return BadRequest(devRes.errors);

            var loginAttempt = await _loginAttemptStore.GetLoginAttemptById(dto.LoginAttemptId);
            if (loginAttempt is null || !loginAttempt.IsValid(_timeWindows.LoginLifetime)) return NotFound(new ErrorDetail { Field = "Two factor email", Reason = "Login attempt dose not exist or is invalid." });

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if (user is null) return NotFound(new ErrorDetail { Field = "Two factor email", Reason = "User dose not exist." });

            var device = await _deviceManager.GetRequestingDevice(user.Id, Request);
            if (device is null || !device.Trusted()) return StatusCode(403, new { redirectUrl = "/deviceConfirme", message = "Device needs confirmation" });

            var (isValid, errors) = await _twoFactorEmailAttemptStore.ValidateAttempt(dto.LoginAttemptId, dto.EmailCode);
            if(!isValid) return BadRequest(errors);

            var roles = await _userManager.GetRolesAsync(user);

            (string accessToken, string tokenId) = _jwtHelper.GenerateToken(user, roles);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            Token token = new()
            {
                Id = tokenId,
                RefreshToken = _stringEncryptionHelper.Hash(refreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(_JWTOptions.RefreshTokenExpiriesInMinutes),
                UserId = user.Id,
                UserDeviceId = device.Id
            };

            await _tokenStore.SetToken(token);

            user.LastLoginDeviceId = device.Id;

            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return BadRequest(res.Errors);

            Response.Cookies.Append(CookieNamesConstant.JWT, accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_JWTOptions.ExpiresInMinutes)
            });


            Response.Cookies.Append(CookieNamesConstant.REFRESH_TOKEN, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_JWTOptions.RefreshTokenExpiriesInMinutes)
            });


            return Ok(new { accessToken, refreshToken });
        }

        [AllowAnonymous]
        [HttpPost("resend-two-factor-email-authentication")]
        public async Task<ActionResult> ReSendTwoFactorEmailAuth([FromBody] ReSendTwoFactorEmailAttemptDto reSendTwoFactorEmailAttemptDto)
        {
            var devRes = _deviceManager.VerifyRequestHasRequiredProperties(Request);
            if (!devRes.isValid) return BadRequest(devRes.errors);

            var loginAttempt = await _loginAttemptStore.GetLoginAttemptById(reSendTwoFactorEmailAttemptDto.LoginAttemptId);
            if(loginAttempt is null || !loginAttempt.IsValid(_timeWindows.LoginLifetime)) return NotFound(new ErrorDetail { Field = "Two factor email", Reason = "Login attempt dose not exist or is invalid."});

            var user = await _userManager.FindByIdAsync(loginAttempt.UserId);
            if(user is null) return NotFound(new ErrorDetail { Field = "Two factor email", Reason = "User dose not exist." });

            var device = await _deviceManager.GetRequestingDevice(user.Id, Request);
            if (device is null || !device.Trusted()) return StatusCode(403, new { redirectUrl = "/deviceConfirme", message = "Device needs confirmation" });

            var attempt = new TwoFactorEmailAttempt
            {
                Code = Guid.NewGuid().ToString(),
                Email = user.Email!,
                LoginAttemptId = loginAttempt.Id,
            };

            var (canMakeAttempt, errors) = await _twoFactorEmailAttemptStore.AddAttempt(attempt);
            if (!canMakeAttempt) return BadRequest(errors);

            // send email

            return Ok(attempt.Code);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var userToCreate = new ApplicationUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.Email,
                PhoneNumber = registerRequestDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(userToCreate, registerRequestDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { id = userToCreate.Id });
        }

        /// <summary>
        /// Accepts a refresh token Validates the refresh token Generates a 
        /// new access token Returns the new access token
        /// </summary>
        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tokenId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            var devRes = _deviceManager.VerifyRequestHasRequiredProperties(Request);
            if (!devRes.isValid) return BadRequest(devRes.errors);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            if (string.IsNullOrEmpty(tokenId))
            {
                return Unauthorized("Jti ID not found in token.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return Unauthorized();

            var device = await _deviceManager.GetRequestingDevice(user.Id, Request);
            if(device is null || !device.IsTrusted) return Unauthorized("Device not registered or trusted.");

            (bool isValid, DateTime? RefreshTokenExpiresAt) = await _tokenStore.ValidateTokenAsync(tokenId, device.Id, _stringEncryptionHelper.Hash(refreshTokenRequestDto.RefreshToken));

            if (!isValid || RefreshTokenExpiresAt is null) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            var (accessToken, accessTokenId) = _jwtHelper.GenerateToken(user, roles);

            Token token = new()
            {
                Id = accessTokenId,
                RefreshToken = refreshTokenRequestDto.RefreshToken,
                RefreshTokenExpiresAt = (DateTime)RefreshTokenExpiresAt,
                UserId = userId,
                UserDeviceId = device.Id
            };

            await _tokenStore.StoreTokenAsync(token);

            return Ok(new { accessToken });
        }

        /// <summary>
        /// Accepts an access token or refresh token Revokes the token, adding it to a blacklist
        /// </summary>
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

            (bool canMakeAttempt,ICollection<ErrorDetail> errors) = await _passwordResetAttemptStore.AddAttempt(passwordResetAttempt);

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

            if(!result.Succeeded) return BadRequest(result.Errors);

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
        [HttpPost("confirm-email")]
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

            (bool canMakeAttempt,ICollection<ErrorDetail> errors) = await _emailVerificationAttemptStore.AddAttemptAsync(attempt);
            if (!canMakeAttempt) return BadRequest(errors);

            // send code in email

            return Ok(attempt.Code);
        }


        [AllowAnonymous]
        [HttpPost("user-device-challenge")]
        public async Task<ActionResult> Challenge([FromBody] UserDeviceChallengeDto challengeDto)
        {
            var user = await _userManager.FindByEmailAsync(challengeDto.Email);
            if (user is null) return Ok("User dose not exist - would not show in prod");

            (bool isValid,UserDeviceChallengeAttempt? attempt) = await _userDeviceChallengeAttemptStore.ValidateAttemptAsync(challengeDto.Email, challengeDto.Code);
            if (!isValid || attempt is null) return BadRequest("Attempt is invalid or attempt not found.");

            attempt.MarkUsed();
            _userDeviceChallengeAttemptStore.SetToUpdateAttempt(attempt);

            var device = await _deviceManager.GetRequestingDevice(user.Id, Request);
            if (device is null) return NotFound("Device not found.");

            device.IsTrusted = true;

            await _userDeviceStore.UpdateAsync(device);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("resend-user-device-challenge")]
        public async Task<ActionResult> ReSendChallenge([FromBody] ReSendUserDeviceChallengeDto challengeDto)
        {
            var (isValid, errors) = _deviceManager.VerifyRequestHasRequiredProperties(Request);
            if (!isValid) return BadRequest(errors);

            var user = await _userManager.FindByEmailAsync(challengeDto.Email);
            if (user is null) return Ok("User dose not exist - would not show in prod");

            var code = Guid.NewGuid().ToString();

            var deviceId = _deviceIdentification.GenerateDeviceId(user.Id, Request.Headers.UserAgent!, Request.HttpContext.Request.Headers[CustomHeaderOptions.XDeviceFingerprint].FirstOrDefault()!);

            var device = await _deviceManager.GetRequestingDevice(user.Id, Request);
            if (device is not null && device.IsTrusted is true) return BadRequest("Device is already trusted - no need to send a challenge.");

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

            (bool canMakeAttempt,ICollection<string> Errors) = await _userDeviceChallengeAttemptStore.AddAttempt(attempt);
            if (!canMakeAttempt) return BadRequest(Errors);

            return Ok(code);
        }

        [AllowAnonymous]
        [HttpPost("phone-confirmation")]
        public async Task<ActionResult> PhoneConfirmation([FromBody] PhoneConfirmationDto phoneConfirmationDto)
        {
            (bool isValid, PhoneConfirmationAttempt? attempt,ICollection<ErrorDetail> errors) = 
                await _phoneConfirmationAttemptStore.ValidateAttempt(phoneConfirmationDto.PhoneNumber, phoneConfirmationDto.Code);

            if (!isValid) return BadRequest(errors);

            if(attempt is null) return Unauthorized();

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
            { Field = "Phone Confirmation", Reason = "Users phone number already confirmed."});

            var attempt = new PhoneConfirmationAttempt
            {
                Code= Guid.NewGuid().ToString(),
                PhoneNumber = user.PhoneNumber!,
                UserId= user.Id,
            };

            (bool canMakeAttempt,ICollection<ErrorDetail> errors) = await _phoneConfirmationAttemptStore.AddAttempt(attempt);
            if (!canMakeAttempt) return BadRequest(errors);

            // send code in sms message

            return Ok(attempt.Code);
        }
    }
}
