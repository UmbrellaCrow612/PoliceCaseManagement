using Identity.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
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
