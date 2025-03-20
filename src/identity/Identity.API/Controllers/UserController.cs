using Identity.API.DTOs;
using Identity.API.Mappings;
using Identity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly UserMapping _userMapping = new();

        /// <summary>
        /// This way to stop 401 being sent on load of app and getting stuck on login page
        /// other endpoints send 401 as normal this endpoint in uniuque for it.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserByIdAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(userId is null) return BadRequest("User id missing");

            var user = await _authService.GetUserByIdAsync(userId);
            if(user is null) return NotFound();

            var roles = await _authService.GetUserRolesAsync(userId);

            var userDto = _userMapping.ToDto(user);
            var returnDto = new MeDto
            {
                Roles = roles,
                User = userDto
            };

            return Ok(returnDto);
        }
    }
}
