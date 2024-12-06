using Identity.API.DTOs;
using Identity.API.Helpers;
using Identity.Infrastructure.Data.Models;
using Identity.Infrastructure.Data.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController(
        JwtHelper jwtHelper, UserManager<ApplicationUser> userManager, IConfiguration configuration,
        StringEncryptionHelper stringEncryptionHelper, ITokenStore tokenStore, IPasswordResetAttemptStore passwordResetAttemptStore, 
        ILogger<AuthenticationController> logger, ILoginAttemptStore loginAttemptStore,
        IEmailVerificationAttemptStore emailVerificationAttemptStore, IUserDeviceStore userDeviceStore,
        IUserDeviceChallengeAttemptStore userDeviceChallengeAttemptStore, IDeviceIdentification deviceIdentification, IPhoneConfirmationAttemptStore phoneConfirmationAttemptStore
        ) : ControllerBase
    {
        private readonly JwtHelper _jwtHelper = jwtHelper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
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

        /// <summary>
        /// Accepts username and password Authenticates the user Generates an access token and 
        /// a refresh token Returns the tokens in the response
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            int refreshTokenExpiriesInMinutes = _configuration.GetValue<int>("Jwt:RefreshTokenExpiriesInMinutes");

            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
                                ?? Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                ?? "Unknown";

            string userAgent = Request.Headers.UserAgent.ToString();

            if (string.IsNullOrWhiteSpace(loginRequestDto.Email) && string.IsNullOrWhiteSpace(loginRequestDto.UserName))
            {
                return BadRequest("Provide a username of email");
            }

            ApplicationUser? user;
            if (!string.IsNullOrWhiteSpace(loginRequestDto.UserName))
            {
                user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
                if (user is null) return Unauthorized("Username or password incorrect");
            }
            else
            {
                user = await _userManager.FindByEmailAsync(loginRequestDto.Email ?? string.Empty);
                if (user is null) return Unauthorized("Email or password incorrect");
            }

            if (user is null) return Unauthorized();

            LoginAttempt loginAttempt = new()
            {
                IpAddress = ipAddress,
                UserAgent = userAgent,
                UserId = user.Id,
            };
           
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
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
                    redirectUrl = "/phoneConfirme",
                    message = "Phone needs confirmation"
                });
            }

            var requestDeviceId = _deviceIdentification.GenerateDeviceId(user.Id, userAgent);

            var device = await _userDeviceStore.GetUserDeviceByIdAsync(user, requestDeviceId);
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

            loginAttempt.Status = LoginStatus.SUCCESS;

            var roles = await _userManager.GetRolesAsync(user);

            (string  accessToken, string tokenId) = _jwtHelper.GenerateToken(user, roles);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            await _loginAttemptStore.SetLoginAttempt(loginAttempt);

            Token token = new()
            {
                Id = tokenId,
                RefreshToken = _stringEncryptionHelper.Hash(refreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(refreshTokenExpiriesInMinutes),
                UserId = user.Id,
                UserDeviceId = device.Id
            };

            await _tokenStore.SetToken(token);

            user.LastLoginDeviceId = device.Id;

            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return BadRequest(res.Errors);

            return Ok(new { accessToken , refreshToken });
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

            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
                              ?? Request.Headers["X-Forwarded-For"].FirstOrDefault()
                              ?? "Unknown";

            string userAgent = Request.Headers.UserAgent.ToString();

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

            var requestDeviceId = _deviceIdentification.GenerateDeviceId(user.Id, userAgent);
            var device = await _userDeviceStore.GetUserDeviceByIdAsync(user, requestDeviceId);
            if(device is null || !device.IsTrusted) return Unauthorized("Device not registered or trusted.");

            (bool isValid, DateTime? RefreshTokenExpiresAt, IEnumerable<string> Errors) = await _tokenStore.ValidateTokenAsync(tokenId, device.Id, _stringEncryptionHelper.Hash(refreshTokenRequestDto.RefreshToken));

            if (!isValid || RefreshTokenExpiresAt is null) return Unauthorized(Errors);

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

            var device = await _userDeviceStore.GetUserDeviceByIdAsync(user, attempt.UserDeviceId);
            if (device is null) return NotFound("Device not found.");

            device.IsTrusted = true;

            await _userDeviceStore.UpdateAsync(device);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("resend-user-device-challenge")]
        public async Task<ActionResult> ReSendChallenge([FromBody] ReSendUserDeviceChallengeDto challengeDto)
        {
            string userAgent = Request.Headers.UserAgent.ToString();

            var user = await _userManager.FindByEmailAsync(challengeDto.Email);
            if (user is null) return Ok("User dose not exist - would not show in prod");

            var code = Guid.NewGuid().ToString();

            var deviceIdentifierId = _deviceIdentification.GenerateDeviceId(user.Id, userAgent);

            UserDevice? device = await _userDeviceStore.GetUserDeviceByIdAsync(user, deviceIdentifierId);
            if (device is not null && device.IsTrusted is true) return BadRequest("Device is already trusted - no need to send a challenge.");

            device ??= new UserDevice
            {
                Id = deviceIdentifierId,
                DeviceName = "Test name",
                IsTrusted = false,
                UserId = user.Id,
            };

            await _userDeviceStore.SetUserDevice(user, device);

            var attempt = new UserDeviceChallengeAttempt
            {
                Code = code,
                Email = challengeDto.Email,
                UserDeviceId = deviceIdentifierId,
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

            var code = Guid.NewGuid().ToString();

            var attempt = new PhoneConfirmationAttempt
            {
                Code= code,
                PhoneNumber = user.PhoneNumber!,
                UserId= user.Id,
            };

            (bool canMakeAttempt,ICollection<ErrorDetail> errors) = await _phoneConfirmationAttemptStore.AddAttempt(attempt);
            if (!canMakeAttempt) return BadRequest(errors);

            // send code in sms message

            return Ok(code);
        }
    }
}
