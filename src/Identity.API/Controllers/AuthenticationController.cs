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
    public class AuthenticationController(JwtHelper jwtHelper, UserManager<ApplicationUser> userManager, IConfiguration configuration, StringEncryptionHelper stringEncryptionHelper, ITokenStore tokenStore, IPasswordResetAttemptStore passwordResetAttemptStore, RoleManager<IdentityRole> roleManager, DeviceInfoHelper deviceInfoHelper) : ControllerBase
    {
        private readonly JwtHelper _jwtHelper = jwtHelper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly StringEncryptionHelper _stringEncryptionHelper = stringEncryptionHelper;
        private readonly ITokenStore _tokenStore = tokenStore;
        private readonly IPasswordResetAttemptStore _passwordResetAttemptStore = passwordResetAttemptStore;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly DeviceInfoHelper _deviceInfoHelper = deviceInfoHelper;

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
                loginAttempt.FailureReason = "User credentials";

                await _tokenStore.StoreLoginAttempt(loginAttempt);

                return Unauthorized("Incorrect credentials");
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

            await _tokenStore.SetDeviceInfo(deviceInfo);
            await _tokenStore.SetLoginAttempt(loginAttempt);

            Token token = new()
            {
                Id = tokenId,
                RefreshToken = _stringEncryptionHelper.Hash(refreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(refreshTokenExpiriesInMinutes),
                UserId = user.Id,
                DeviceInfoId = deviceInfo.Id,
            };

            await _tokenStore.StoreTokenAsync(token);

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

            var result = await _tokenStore.ValidateTokenAsync(tokenId, _stringEncryptionHelper.Hash(refreshTokenRequestDto.RefreshToken));

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

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

            await _tokenStore.SetDeviceInfo(deviceInfo);

            Token token = new()
            {
                Id = accessTokenId,
                RefreshToken = refreshTokenRequestDto.RefreshToken,
                RefreshTokenExpiresAt = result.RefreshTokenExpiresAt,
                UserId = userId,
                DeviceInfoId = deviceInfo.Id
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
            int resetPasswordSessionTimeInMinutes = int.Parse(_configuration["ResetPasswordSessionTimeInMinutes"] ?? throw new ApplicationException("ResetPasswordSessionTimeInMinutes not provided."));
            int resetPasswordCodeLength = int.Parse(_configuration["ResetPasswordCodeLength"] ?? throw new ApplicationException("ResetPasswordCodeLength not provided."));

            var user = await _userManager.FindByEmailAsync(resetPasswordRequestDto.Email);
            if (user is null) return Ok(); // We don't reveal if a user exists

            var trueCode = _stringEncryptionHelper.GenerateRandomString(resetPasswordCodeLength);

            PasswordResetAttempt passwordResetAttempt = new()
            {
                Code = _stringEncryptionHelper.Hash(trueCode), 
                UserId = user.Id,
                ValidSessionTime = DateTime.UtcNow.AddMinutes(resetPasswordSessionTimeInMinutes),
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
            (bool isValid, PasswordResetAttempt? attempt) = await _passwordResetAttemptStore.ValidateAttempt(_stringEncryptionHelper.Hash(code));
            if (!isValid) return Unauthorized(); // Either session expired or code is wrong for latest attempt

            if (attempt is null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(attempt.UserId);
            if (user is null) return BadRequest();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, confirmPasswordResetRequestDto.NewPassword);

            if(!result.Succeeded) return BadRequest(result.Errors);

            attempt.IsSuccessful = true;

            await _passwordResetAttemptStore.UpdateAttempt(attempt);

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
        [HttpGet("confirmEmail")]
        public async Task<ActionResult> ConfirmEmail()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("resendConfirmationEmail")]
        public async Task<ActionResult> ResendConfirmEmail()
        {
            return Ok();
        }
    }
}
