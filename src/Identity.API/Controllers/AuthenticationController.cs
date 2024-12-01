using Identity.API.DTOs;
using Identity.API.Helpers;
using Identity.Infrastructure.Data.Models;
using Identity.Infrastructure.Data.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UAParser;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController(
        JwtHelper jwtHelper, UserManager<ApplicationUser> userManager, IConfiguration configuration,
        StringEncryptionHelper stringEncryptionHelper, ITokenStore tokenStore, IPasswordResetAttemptStore passwordResetAttemptStore, 
        DeviceInfoHelper deviceInfoHelper, ILogger<AuthenticationController> logger, ILoginAttemptStore loginAttemptStore,
        IDeviceInfoStore deviceInfoStore, IEmailVerificationAttemptStore emailVerificationAttemptStore, IUserDeviceStore userDeviceStore
        ) : ControllerBase
    {
        private readonly JwtHelper _jwtHelper = jwtHelper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly StringEncryptionHelper _stringEncryptionHelper = stringEncryptionHelper;
        private readonly ITokenStore _tokenStore = tokenStore;
        private readonly IPasswordResetAttemptStore _passwordResetAttemptStore = passwordResetAttemptStore;
        private readonly DeviceInfoHelper _deviceInfoHelper = deviceInfoHelper;
        private readonly ILogger<AuthenticationController> _logger = logger;
        private readonly ILoginAttemptStore _loginAttemptStore = loginAttemptStore;
        private readonly IDeviceInfoStore _deviceInfoStore = deviceInfoStore;
        private readonly IEmailVerificationAttemptStore _emailVerificationAttemptStore = emailVerificationAttemptStore;
        private readonly IUserDeviceStore _userDeviceStore = userDeviceStore;

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

            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);

            if (string.IsNullOrWhiteSpace(loginRequestDto.Email) && string.IsNullOrWhiteSpace(loginRequestDto.UserName))
            {
                return BadRequest("Provide a username of email");
            }

            _logger.LogInformation("Login attempt by user: {Username} from IP: {IpAddress}, User-Agent: {UserAgent}",
                                    loginRequestDto.UserName ?? loginRequestDto.Email, ipAddress, userAgent);

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
                IpAddress = ipAddress ?? "Unkown",
                UserAgent = userAgent,
                UserId = user.Id,
            };
           
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!isPasswordCorrect)
            {
                _logger.LogWarning("Failed login attempt for user: {Username} from IP: {IpAddress}, Reason: Incorrect credentials",
                                    loginRequestDto.UserName ?? loginRequestDto.Email, ipAddress);

                loginAttempt.FailureReason = "User credentials";

                await _loginAttemptStore.StoreLoginAttempt(loginAttempt);

                return Unauthorized("Incorrect credentials");
            }

            if(!user.EmailConfirmed)
            {
                return StatusCode(403, new
                {
                    redirectUrl = "/emailConfirm",
                    message = "Email needs confirmation"
                });

            }

            var userDevice = await _userDeviceStore.GetUserDeviceByIdAsync(user, _deviceInfoHelper.GenerateDeviceId(clientInfo, ipAddress ?? "Unkown"));
            if(userDevice is null || !userDevice.IsTrusted)
            {
                // new device or not trausted so send challegnge code in email
                return BadRequest("Challenge made.");
            }

            if (user.LockoutEnabled is true && user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                return Unauthorized("User account locked.");
            }

            loginAttempt.Status = LoginStatus.SUCCESS;

            var roles = await _userManager.GetRolesAsync(user);

            (string  accessToken, string tokenId) = _jwtHelper.GenerateToken(user, roles);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            DeviceInfo deviceInfo = new()
            {
                IpAddress = ipAddress ?? "Not Found",
                TokenId = tokenId,
                UserAgent = userAgent,
                Browser = clientInfo.UA.Family,
                Os = clientInfo.OS.Family,
                DeviceType = _deviceInfoHelper.DetermineDeviceType(clientInfo),
                DeviceId = _deviceInfoHelper.GenerateDeviceId(clientInfo, ipAddress ?? "Unkown")
            };

            await _deviceInfoStore.SetDeviceInfo(deviceInfo);
            await _loginAttemptStore.SetLoginAttempt(loginAttempt);

            Token token = new()
            {
                Id = tokenId,
                RefreshToken = _stringEncryptionHelper.Hash(refreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(refreshTokenExpiriesInMinutes),
                UserId = user.Id,
                DeviceInfoId = deviceInfo.Id,
            };

            await _tokenStore.SetToken(token);

            user.LastLoginDeviceId = deviceInfo.Id;

            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return BadRequest(res.Errors);

            _logger.LogInformation("Successful login for user: {Username} from IP: {IpAddress}",
                                    user.UserName, ipAddress);

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

            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);


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

            (bool isValid, DateTime? RefreshTokenExpiresAt, IEnumerable<string> Errors) = await _tokenStore.ValidateTokenAsync(tokenId, _stringEncryptionHelper.Hash(refreshTokenRequestDto.RefreshToken));

            if (!isValid || RefreshTokenExpiresAt is null) return Unauthorized(Errors);

            var roles = await _userManager.GetRolesAsync(user);

            var (accessToken, accessTokenId) = _jwtHelper.GenerateToken(user, roles);

            DeviceInfo deviceInfo = new()
            {
                IpAddress = ipAddress ?? "Not Found",
                TokenId = accessTokenId,
                UserAgent = userAgent,
                Browser = clientInfo.UA.Family,
                Os = clientInfo.OS.Family,
                DeviceType = _deviceInfoHelper.DetermineDeviceType(clientInfo),
                DeviceId = _deviceInfoHelper.GenerateDeviceId(clientInfo, ipAddress ?? "Unkown")
            };

            await _deviceInfoStore.SetDeviceInfo(deviceInfo);

            Token token = new()
            {
                Id = accessTokenId,
                RefreshToken = refreshTokenRequestDto.RefreshToken,
                RefreshTokenExpiresAt = (DateTime)RefreshTokenExpiresAt,
                UserId = userId,
                DeviceInfoId = deviceInfo.Id,
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

            var (canMakeAttempt, successfullyAdded) = await _passwordResetAttemptStore.AddAttempt(passwordResetAttempt);

            if (!canMakeAttempt)
            {
                return BadRequest(); // Already made one in time period
            }

            if (!successfullyAdded)
            {
                return BadRequest();
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

            if (attempt is null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(attempt.UserId);
            if (user is null) return BadRequest();

            attempt.IsSuccessful = true;
            attempt.SuccessfulAt = DateTime.UtcNow;

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

            var attempt = new EmailVerificationAttempt()
            {
                Email = resendConfirmationEmailDto.Email,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                Code = Guid.NewGuid().ToString(),
                UserId = user.Id
            };

            (bool canMakeAttempt,bool successfullyAdded) = await _emailVerificationAttemptStore.AddAttemptAsync(attempt);
            if (!canMakeAttempt || !successfullyAdded) return BadRequest();

            // send code in email

            return Ok(attempt.Code);
        }


        [AllowAnonymous]
        [HttpPost("challenge")]
        public async Task<ActionResult> Challenge([FromBody] ChallengeDto challengeDto)
        {
            // on sucess add the device to the sotre lse fail response
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("resend-challenge")]
        public async Task<ActionResult> ReSendChallenge([FromBody] ReSendChallengeDto challengeDto)
        {
            // check for if there is a valid attempt out if not then send a new one.
            return Ok();
        }
    }
}
