using AutoMapper;
using Identity.API.DTOs;
using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController(UserManager<ApplicationUser> userManager, ITokenStore tokenStore, IMapper mapper) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ITokenStore _tokenStore = tokenStore;
        private readonly IMapper _mapper = mapper;

        [Authorize]
        [HttpPost("{userId}/lock")]
        public async Task<ActionResult> LockUserById(string userId, [FromBody] DateTime LockoutEnd)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null) return NotFound("User not found.");

            if (LockoutEnd < DateTime.UtcNow) return BadRequest("Lockout end must be in the future.");

            if (user.LockoutEnabled is true && user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                return BadRequest($"User account is already locked until {user.LockoutEnd}");
            }

            user.LockoutEnd = LockoutEnd;

            await _tokenStore.SetRevokeAllUserTokensAsync(user);

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{userId}/un-lock")]
        public async Task<ActionResult> UnLockUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound("User not found.");

            if (user.LockoutEnabled is true && user.LockoutEnd.HasValue && user.LockoutEnd < DateTime.UtcNow)
            {
                return BadRequest($"User account is already un-locked.");
            }

            user.LockoutEnd = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound("User not found.");

            var dto = _mapper.Map<UserDto>(user);

            return Ok(dto);
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
