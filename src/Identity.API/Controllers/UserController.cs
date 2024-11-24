using Identity.API.DTOs;
using Identity.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController(UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [Authorize]
        [HttpPost("{userId}/lock")]
        public async Task<ActionResult> LockUserById(string userId, [FromBody] DateTime LockoutEnd)
        {
            // TODO - use the lockout endbaled and lockout end - which will be used in login endpoint
            // also revoke user tokens
            return Ok();
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserById(string userId)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> SearchUsers([FromQuery] SearchUsersQuery query)
        {
            return Ok();
        }

        [Authorize]
        [HttpPatch("{userId}")]
        public async Task<ActionResult> PatchUserById()
        {
            return Ok();
        }

        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUserById(string userId)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("{userId}/logins")]
        public async Task<ActionResult> GetUserLogins(string userId)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("{userId}/passwordResetAttempts")]
        public async Task<ActionResult> GetUserPasswordResetAttempts(string userId)
        {
            return Ok();
        }
    }
}
