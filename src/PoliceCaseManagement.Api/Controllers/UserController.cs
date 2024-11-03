using Microsoft.AspNetCore.Mvc;
using PoliceCaseManagement.Application.DTOs.Users;
using PoliceCaseManagement.Application.Interfaces;

namespace PoliceCaseManagement.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto request)
        {
            var createdUser = await _userService.CreateUserAsync(request);

            return Created(nameof(CreateUser), createdUser);
        }

        [HttpPost("{id}/roles")]
        public async Task<ActionResult> AddRolesToUser(string id, [FromBody] IEnumerable<string> roles)
        {
            await _userService.LinkUserToRolesAsync(id, roles);

            return Ok(roles);
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult> GetUserRoles(string id)
        {
            var roles = await _userService.GetUserRolesAsync(id);

            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(string id)
        {
            var userToGet = await _userService.GetUserByIdAsync(id);

            return userToGet is null ? NotFound() : Ok(userToGet);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateUserById(string id, [FromBody] UpdateUserDto request)
        {
            return await _userService.UpdateUserByIdAsync(id, request) ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserById(string id)
        {
            var userId = "1";

            await _userService.DeleteUserByIdAsync(id, userId);

            return NoContent();
        }

        [HttpDelete("{id}/roles")]
        public async Task<ActionResult> DeleteRolesFromUser(string id, [FromBody] IEnumerable<string> roles)
        {
            await _userService.UnLinkUserFromRolesAsync(id, roles);

            return NoContent();
        } 
    }
}
