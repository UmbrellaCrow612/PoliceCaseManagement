using AutoMapper;
using Identity.API.DTOs;
using Identity.Infrastructure.Data.Models;
using Identity.Infrastructure.Data.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("departments")]
    public class DepartmentController(IDepartmentStore departmentStore, UserManager<ApplicationUser> userManager, ILogger<DepartmentController> logger, IMapper mapper) : ControllerBase
    {
        private readonly IDepartmentStore _departmentStore = departmentStore;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<DepartmentController> _logger = logger;
        private readonly IMapper _mapper = mapper;

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
        {
            var department = _mapper.Map<Department>(createDepartmentDto);
            await _departmentStore.StoreDepartment(department);

            _logger.LogInformation("Department {id} was created", department.Id);

            return Ok(new { id = department.Id });
        }

        [Authorize]
        [HttpPost("{departmentId}/users/{userId}")]
        public async Task<ActionResult> AddUserToDepartment(string departmentId, string userId)
        {
            var department = await _departmentStore.GetDepartmentById(departmentId);
            if(department is null) return NotFound("Department not found.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound("User not found.");

            if (await _departmentStore.IsUserInDepartment(department, user))
            {
                return BadRequest("User already in department");
            }

            await _departmentStore.AddUser(department, user);

            _logger.LogInformation("User {id} added to department {depId}", user.Id, department.Id);

            return NoContent();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> SearchDepartments([FromQuery] SearchDepartmentQuery query)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("{departmentId}")]
        public async Task<ActionResult> GetDepartmentById(string departmentId)
        {
            var department = await _departmentStore.GetDepartmentById(departmentId);
            if (department is null) return NotFound("Department not found.");

            var dto = _mapper.Map<DepartmentDto>(department);

            return Ok(dto);
        }

        [Authorize]
        [HttpGet("{departmentId}/users")]
        public async Task<ActionResult> GetDepartmentUsers(string departmentId)
        {
            var department = await _departmentStore.GetDepartmentById(departmentId);
            if (department is null) return NotFound("Department not found.");

            var users = await _departmentStore.GetUsers(department);

            var dto = _mapper.Map<IEnumerable<UserDto>>(users);

            return Ok(dto);
        }

        [Authorize]
        [HttpPatch("{departmentId}")]
        public async Task<ActionResult> UpdateDepartmentById(string departmentId)
        {
            return Ok();
        }

        [Authorize]
        [HttpDelete("{departmentId}")]
        public async Task<ActionResult> DeleteDepartmentById(string departmentId)
        {
            return Ok();
        }

        [Authorize]
        [HttpDelete("{departmentId}/users/{userId}")]
        public async Task<ActionResult> RemoveUserFromDepartment(string departmentId, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound("User not found.");

            var department = await _departmentStore.GetDepartmentById(departmentId);
            if (department is null) return NotFound("Department not found.");

            if (!await _departmentStore.IsUserInDepartment(department, user))
            {
                return BadRequest("User is not in department.");
            }

            await _departmentStore.RemoveUser(department, user);

            return NoContent();
        }
    }
}
