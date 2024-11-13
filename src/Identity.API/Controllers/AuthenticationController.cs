﻿using Identity.API.DTOs;
using Identity.API.Helpers;
using Identity.Infrastructure.Data.Models;
using Identity.Infrastructure.Data.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController(JwtHelper jwtHelper, UserManager<ApplicationUser> userManager, IConfiguration configuration, StringEncryptionHelper stringEncryptionHelper, ITokenStore tokenStore) : ControllerBase
    {
        private readonly JwtHelper _jwtHelper = jwtHelper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly StringEncryptionHelper _stringEncryptionHelper = stringEncryptionHelper;
        private readonly ITokenStore _tokenStore = tokenStore;
       
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
                AccessToken = accessToken,
                Id = tokenId,
                RefreshToken = _stringEncryptionHelper.Hash(refreshToken),
                RefreshTokenExpiriesAt = DateTime.UtcNow.AddMinutes(refreshTokenExpiriesInMinutes),
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

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null || user.RefreshToken is null || user.RefreshTokenExpiriesAt is null) return Unauthorized();

            if (user.RefreshTokenExpiriesAt <= DateTime.UtcNow || user.RefreshToken != _stringEncryptionHelper.Hash(refreshTokenRequestDto.RefreshToken))
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiriesAt = null;

                await _userManager.UpdateAsync(user);

                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtHelper.GenerateToken(user, roles);

            return Ok(new { token.accessToken });
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

            user.RefreshToken = null;
            user.RefreshTokenExpiriesAt = null;

            await _userManager.UpdateAsync(user);

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
        [HttpGet("sec")]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
