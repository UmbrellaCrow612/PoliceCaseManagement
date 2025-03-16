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

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserByIdAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(userId is null) return Unauthorized("User id missing");

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
