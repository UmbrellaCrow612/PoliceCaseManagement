using System.Security.Claims;
using Authorization;
using Cache.Abstractions;
using Identity.API.DTOs;
using Identity.API.Mappings;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagination.Abstractions;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController(
        IUserService userService, 
        ICache cache, 
        IRoleService roleService,
        ITotpService totpService
        ) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly UserMapping _userMapping = new();
        private readonly ICache _cache = cache;
        private readonly IRoleService _roleService = roleService;
        private readonly ITotpService _totpService = totpService;

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserByIdAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return BadRequest("User id missing");

            var value = await _cache.GetAsync<UserMeResponseDto>(userId);
            if (value is not null)
            {
                return Ok(value);
            }

            var user = await _userService.FindByIdAsync(userId);
            if (user is null) return Unauthorized();

            var roles = await _roleService.GetRolesAsync(user);

            var userDto = _userMapping.ToDto(user);
            var returnDto = new UserMeResponseDto
            {
                Roles = [.. roles.Select(x => x.Name)],
                User = userDto
            };

            await _cache.SetAsync<UserMeResponseDto>(userId, returnDto);
            return Ok(returnDto);
        }
     
        [HttpPost("usernames/is-taken")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsUsernameTaken([FromBody] UsernameTakenDto dto)
        {
            var taken = await _userService.IsUsernameTakenAsync(dto.Username);

            return Ok(new UsernameTakenResponseDto{ Taken = taken });
        }

        [HttpPost("emails/is-taken")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> IsEmailTaken([FromBody] EmailTakenDto dto)
        {
            var taken = await _userService.IsEmailTakenAsync(dto.Email);

            return Ok(new EmailTakenResponseDto { Taken = taken });
        }

        [HttpPost("phone-numbers/is-taken")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<PhoneNumberTakenResponseDto>> IsPhoneNumberTaken([FromBody] PhoneNumberTakenDto dto)
        {
            var taken = await _userService.IsPhoneNumberTakenAsync(dto.PhoneNumber);

            return Ok(new PhoneNumberTakenResponseDto { Taken = taken });
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            var user = await _userService.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }
            var dto = _userMapping.ToDto(user);

            return Ok(dto);
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateUserByIdAsync(string userId, [FromBody] UpdateUserDto dto)
        {
            var user = await _userService.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            _userMapping.Update(user, dto);

            var result = await _userService.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _cache.DeleteAsync(userId);
            return NoContent();
        }

        [HttpGet("{userId}/roles")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUserRolesAsync(string userId)
        {
            var user = await _userService.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            var roles = await _roleService.GetRolesAsync(user);

            return Ok(new { roles });
        }

        [HttpGet("admin/search")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AdminSearchUsersAsync([FromQuery] SearchUserQuery query)
        {
            var paginatedResult = await _userService.SearchAsync(query);

            var dto = new PaginatedResult<UserDto>
            {
                Data = [.. paginatedResult.Data.Select(x => _userMapping.ToDto(x))],
                HasNextPage = paginatedResult.HasNextPage,
                HasPreviousPage = paginatedResult.HasPreviousPage,
                Pagination = paginatedResult.Pagination,
            };

            return Ok(dto);
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> SearchUsersAsync([FromQuery] SearchUserQuery query)
        {
            var paginatedResult = await _userService.SearchAsync(query);

            var dto = new PaginatedResult<RestrictedUserDto>
            {
                Data = [.. paginatedResult.Data.Select(x => _userMapping.ToRestrictedUserDto(x))],
                HasNextPage = paginatedResult.HasNextPage,
                HasPreviousPage = paginatedResult.HasPreviousPage,
                Pagination = paginatedResult.Pagination,
            };

            return Ok(dto);
        }

        [Authorize]
        [HttpGet("totp")]
        public async Task<IActionResult> CreateTotpForMe()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return BadRequest("User id missing");

            var user = await _userService.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            var result = await _totpService.GenerateTotp(user);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(new { result.QrCodeBytes });
        }

        [Authorize]
        [HttpDelete("totp")]
        public async Task<IActionResult> ResetTotp()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return BadRequest("User id missing");

            var user = await _userService.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            var result = await _totpService.ResetTotp(user);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }
    }
}
