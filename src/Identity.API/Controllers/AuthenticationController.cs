using Identity.API.DTOs;
using Identity.API.Helpers;
using Identity.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController(JwtHelper jwtHelper, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly JwtHelper _jwtHelper = jwtHelper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        /// <summary>
        /// Accepts username and password Authenticates the user Generates an access token and 
        /// a refresh token Returns the tokens in the response
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (string.IsNullOrWhiteSpace(loginRequestDto.Email) || string.IsNullOrWhiteSpace(loginRequestDto.UserName))
            {
                return BadRequest("Provide a username of email");
            }

            ApplicationUser? user;

            if (!string.IsNullOrWhiteSpace(loginRequestDto.UserName))
            {
                user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
                if(user is null) return Unauthorized("Username of password incorrect");

                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if(!isPasswordCorrect) return Unauthorized("Username of password incorrect");

                var roles = await _userManager.GetRolesAsync(user);

                var token = _jwtHelper.GenerateToken(user, roles);

                return Ok(new { accessToken = token });
               
            }

            if (!string.IsNullOrWhiteSpace(loginRequestDto.Email))
            {
                user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
                if (user is null) return Unauthorized("Email of password incorrect");

                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (!isPasswordCorrect) return Unauthorized("Email of password incorrect");

                var roles = await _userManager.GetRolesAsync(user);

                var token = _jwtHelper.GenerateToken(user, roles);

                return Ok(new { accessToken = token });
            }

            return BadRequest("Username or email not provided");
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
        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken()
        {
            return Ok();
        }

        /// <summary>
        /// Accepts an access token or refresh token Revokes the token, adding it to a blacklist
        /// </summary>
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            return Ok();
        }

        /// <summary>
        /// Requires a valid access token Returns information about the currently authenticated user
        /// </summary>
        [HttpGet("me")]
        public async Task<ActionResult> Me()
        {
            return Ok();
        }

        /// <summary>
        /// Accepts an access token or refresh token Revokes the specified token, adding it to a blacklist
        /// </summary>
        [HttpPost("revoke-token")]
        public async Task<ActionResult> RevokeToken()
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("sec")]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
