using Identity.Api.DTOs;
using Identity.Api.Jwt;
using Identity.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

            ApplicationUser? user = null;

            if (loginDto.Email is not null)
            {
                user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user is null) return NotFound("User not found.");

                var isApproved = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isApproved) return Unauthorized("Email or password incorrect.");
            }

            if (loginDto.Username is not null)
            {
                user = await _userStore.FindByNameAsync(loginDto.Username, CancellationToken.None);
                if (user is null) return NotFound("User not found.");

                var isApproved = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isApproved) return Unauthorized("Username or password incorrect.");
            }

            var userRoles = await _userManager.GetRolesAsync(user!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user?.Id!),
                new Claim(ClaimTypes.Name, user?.UserName!),
                new Claim(ClaimTypes.Email, user?.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var (AccessToken, RefreshToken) = _jwtService.GenerateTokens(claims);

            return Ok(new { acessToken = AccessToken, refreshToken = RefreshToken });
        }
    }
}
