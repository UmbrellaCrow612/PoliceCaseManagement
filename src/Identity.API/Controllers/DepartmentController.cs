using Identity.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("departments")]
    public class DepartmentController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateDepartment()
        {
            return Ok();
        }

        [Authorize]
        [HttpPost("{departmentId}/users/{userId}")]
        public async Task<ActionResult> AddUserToDepartment(string departmentId, string userId)
        {
            // assign one user
            return Ok();
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
            return Ok();
        }

        [Authorize]
        [HttpGet("{departmentId}/users")]
        public async Task<ActionResult> GetDepartmentUsers(string departmentId)
        {
            return Ok();
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
            return Ok();
        }
    }
}
