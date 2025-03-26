using Authorization.Core;
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

        /// <summary>
        /// Used when creating a user to be hit while a admin types to create a user in the system
        /// typically during there typing so they can know early on without having to hit the register endpoint
        /// </summary>
        [HttpPost("usernames/is-taken")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsUsernameTaken([FromBody] IsUsernameTakenDto dto)
        {
            var result = await _authService.IsUsernameTaken(dto.Username);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        /// <summary>
        /// Used when creating a user to be hit while a admin types to create a user in the system
        /// typically during there typing so they can know early on without having to hit the register endpoint
        /// </summary>
        [HttpPost("emails/is-taken")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsEmailTaken([FromBody] IsEmailTakenDto dto)
        {
            var result = await _authService.IsEmailTaken(dto.Email);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        /// <summary>
        /// Used when creating a user to be hit while a admin types to create a user in the system
        /// typically during there typing so they can know early on without having to hit the register endpoint
        /// </summary>
        [HttpPost("phone-numbers/is-taken")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsPhoneNumberTaken([FromBody] IsPhoneNumberTakenDto dto)
        {
            var result = await _authService.IsPhoneNumberTaken(dto.PhoneNumber);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}
