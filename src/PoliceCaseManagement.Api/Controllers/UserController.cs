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
    }
}
