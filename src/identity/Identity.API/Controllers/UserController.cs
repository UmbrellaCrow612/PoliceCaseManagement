using Authorization;
using Caching;
using Identity.API.DTOs;
using Identity.API.Mappings;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController(IAuthService authService, IRedisService redisService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly UserMapping _userMapping = new();
        private readonly IRedisService _redisService = redisService;


        /// <summary>
        /// This way to stop 401 being sent on load of app and getting stuck on login page
        /// other endpoints send 401 as normal this endpoint in unique for it.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserByIdAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return BadRequest("User id missing");

            string meKey = $@"me_{userId}";
            var value = await _redisService.GetStringAsync<MeDto>(meKey);
            if (value is not null)
            {
                return Ok(value);
            }

            var user = await _authService.GetUserByIdAsync(userId);
            if (user is null) return NotFound();

            var roles = await _authService.GetUserRolesAsync(userId);

            var userDto = _userMapping.ToDto(user);
            var returnDto = new MeDto
            {
                Roles = roles,
                User = userDto
            };

            await _redisService.SetStringAsync<MeDto>(meKey, returnDto);
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
        public async Task<ActionResult<UserDto>> IsPhoneNumberTaken([FromBody] IsPhoneNumberTakenDto dto)
        {
            var result = await _authService.IsPhoneNumberTaken(dto.PhoneNumber);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            var user = await _authService.GetUserByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }
            var dto = _userMapping.ToDto(user);

            return Ok(dto);
        }

        [HttpPatch("{userId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateUserByIdAsync(string userId, [FromBody] UpdateUserDto dto)
        {
            var user = await _authService.GetUserByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            _userMapping.Update(user, dto);
            var result = await _authService.UpdateUserAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpGet("{userId}/roles")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUserRolesAsync(string userId)
        {
            var roles = await _authService.GetUserRolesAsync(userId);

            var dto = new { roles };

            return Ok(dto);
        }

        [HttpPatch("{userId}/roles")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateUserRolesByIdAsync(string userId, UpdateUserRolesDto dto)
        {
            var user = await _authService.GetUserByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            var result = await _authService.UpdateUserRolesAsync(user, dto.Roles);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        /// <summary>
        /// Searching a user - admin - more privilege can see more details about users.
        /// </summary>
        [HttpGet("admin/search")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<List<UserDto>>> AdminSearchUsersAsync([FromQuery] SearchUserQuery query)
        {
            var users = await _authService.SearchUsersByQuery(query);

            List<UserDto> dto = [];
            foreach (var user in users)
            {
                dto.Add(_userMapping.ToDto(user));
            }

            return Ok(dto);
        }

        /// <summary>
        /// Search users - see less details about them for less privileged users.
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> SearchUsersAsync([FromQuery] SearchUserQuery query)
        {
            var users = await _authService.SearchUsersByQuery(query);

            List<RestrictedUserDto> dto = [];
            foreach (var user in users)
            {
                dto.Add(_userMapping.ToRestrictedUserDto(user));
            }

            return Ok(dto);
        }
    }
}
