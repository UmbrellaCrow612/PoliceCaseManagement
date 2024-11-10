using Identity.Api.DTOs;
using Identity.Api.Helpers;
using Identity.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, JwtHelper jwtHelper) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly JwtHelper _jwtHelper = jwtHelper;

        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto.Email is null && loginDto.UserName is null) return BadRequest("Provide email or username for login");

            ApplicationUser? user;

            if (!string.IsNullOrWhiteSpace(loginDto.Email))
            {
                user = await _userManager.FindByEmailAsync(loginDto.Email);
                if(user is null) return NotFound("User with email dose not exist");

                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isPasswordCorrect) return Unauthorized("Email or password incorrect");

                var roles = await _userManager.GetRolesAsync(user);

                var token = _jwtHelper.GenerateToken(user, roles);

                return Ok(new { accessToken = token });
            } 

            if(!string.IsNullOrWhiteSpace(loginDto.UserName))
            {
                user = await _userManager.FindByNameAsync(loginDto.UserName);
                if (user is null) return NotFound("User with username dose not exist");

                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isPasswordCorrect) return Unauthorized("Username or password incorrect");

                var roles = await _userManager.GetRolesAsync(user);

                var token = _jwtHelper.GenerateToken(user, roles);

                return Ok(new { accessToken = token });
            }

            return BadRequest("Failed Login");
        }

        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)] 
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            ApplicationUser userToCreate = new()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(userToCreate, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return CreatedAtAction(nameof(Register), new { id = userToCreate.Id });
        }

        [Authorize]
        [HttpGet("sec")]
        public async Task<ActionResult> Secure()
        {
            return Ok();
        }
    }
}
