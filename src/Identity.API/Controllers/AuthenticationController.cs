using Identity.Api.DTOs;
using Identity.Api.Jwt;
using Identity.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController(JwtService jwtService, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, RoleManager<IdentityRole> roleManager) : ControllerBase
    {
        private readonly JwtService _jwtService = jwtService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserStore<ApplicationUser> _userStore = userStore;
        private readonly IUserEmailStore<ApplicationUser> _userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) && string.IsNullOrEmpty(loginDto.Username)) return BadRequest("Must provide either a username or email in login request.");
            if (loginDto.Email is not null && loginDto.Username is not null) return BadRequest("Do not provide both username and email just one.");

            // Flow for email
            if (loginDto.Email is not null)
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user is null) return NotFound("User not found.");

                // Generate claims and write token out.
            }

            // Flow for username
            if (loginDto.Username is not null)
            {
                var user = await _userStore.FindByNameAsync(loginDto.Username, CancellationToken.None);
                if (user is null) return NotFound("User not found.");

                // Generate claims and write token
            }

            return Ok();
        }
    }
}
