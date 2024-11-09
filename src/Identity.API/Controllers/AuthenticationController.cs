using Identity.Api.DTOs;
using Identity.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, RoleManager<IdentityRole> roleManager) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserStore<ApplicationUser> _userStore = userStore;
        private readonly IUserEmailStore<ApplicationUser> _userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) is not null || await _userManager.FindByNameAsync(registerDto.UserName) is not null)
                return BadRequest("Username or Email already taken");


            ApplicationUser userToCreate = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(userToCreate, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result);

            return Created(nameof(Register), new { id = userToCreate.Id });
        }
    }
}
