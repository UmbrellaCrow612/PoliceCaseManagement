using Identity.API.DTOs;
using Identity.API.Helpers;
using Identity.Infrastructure.Data.Models;
using Identity.Infrastructure.Data.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController(JwtHelper jwtHelper, UserManager<ApplicationUser> userManager, IConfiguration configuration, StringEncryptionHelper stringEncryptionHelper, ITokenStore tokenStore, IPasswordResetAttemptStore passwordResetAttemptStore, RoleManager<IdentityRole> roleManager) : ControllerBase
    {
        private readonly JwtHelper _jwtHelper = jwtHelper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly StringEncryptionHelper _stringEncryptionHelper = stringEncryptionHelper;
        private readonly ITokenStore _tokenStore = tokenStore;
        private readonly IPasswordResetAttemptStore _passwordResetAttemptStore = passwordResetAttemptStore;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;


        /// <summary>
        /// Accepts username and password Authenticates the user Generates an access token and 
        /// a refresh token Returns the tokens in the response
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (string.IsNullOrWhiteSpace(loginRequestDto.Email) && string.IsNullOrWhiteSpace(loginRequestDto.UserName))
            {
                return BadRequest("Provide a username of email");
            }

            int refreshTokenExpiriesInMinutes = _configuration.GetValue<int>("Jwt:RefreshTokenExpiriesInMinutes");

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

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!isPasswordCorrect) return Unauthorized("Incorrect credentials");

            var roles = await _userManager.GetRolesAsync(user);

            (string  accessToken, string tokenId) = _jwtHelper.GenerateToken(user, roles);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            Token token = new()
            {
                Id = tokenId,
                RefreshToken = _stringEncryptionHelper.Hash(refreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(refreshTokenExpiriesInMinutes),
                UserId = user.Id,
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

            Token token = new()
            {
                Id = accessTokenId,
                RefreshToken = refreshTokenRequestDto.RefreshToken,
                RefreshTokenExpiresAt = result.RefreshTokenExpiresAt,
                UserId = userId,
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

        /// <summary>
        /// Requires a valid access token Returns information about the currently authenticated user
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult> Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return Unauthorized();

            return Ok(new { id = user.Id, userName = user.UserName, email = user.Email, phoneNumber = user.PhoneNumber });
        }

        [Authorize]
        [HttpDelete("clean-expired-tokens")]
        public async Task<ActionResult> CleanTokens()
        {
            var count = await _tokenStore.CleanupExpiredTokensAsync();

            return Ok(count);
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
        [HttpPost("users/{id}/roles")]
        public async Task<ActionResult> AddRolesToUser(string id, [FromBody] IEnumerable<string> roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound("User not found.");

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return NotFound("Role not found");
                }

                if (!await _userManager.IsInRoleAsync(user, role))
                {
                    return BadRequest("User already linked to role.");
                }
            }

            await _userManager.AddToRolesAsync(user, roles);

            return NoContent();
        }

        [HttpGet("roles")]
        public async Task<ActionResult> GetSystemRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return Ok(roles);
        }

        [Authorize]
        [HttpGet("sec")]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
