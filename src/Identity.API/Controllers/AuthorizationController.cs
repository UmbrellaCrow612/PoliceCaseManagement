using Identity.Infrastructure.Data.Models;
using Identity.Infrastructure.Data.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("authorization")]
    public class AuthorizationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AuthorizationController> logger, ISecurityAuditStore securityAuditStore) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly ILogger<AuthorizationController> _logger = logger;
        private readonly ISecurityAuditStore _securityAuditStore = securityAuditStore;

        [Authorize]
        [HttpPost("roles/users/{userId}")]
        public async Task<ActionResult> AssignUserRoles([FromBody] IEnumerable<string> roles, string userId)
        {
            var userIdOfRequester = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdOfRequester)) return BadRequest("Jwt Name Identifier is null.");

            var user = await _userManager.FindByIdAsync(userId);
            if(user is null) return NotFound("User not found.");
            
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return NotFound($"{role} dose not exist.");
                }

                if (await _userManager.IsInRoleAsync(user, role))
                {
                    return BadRequest($"User is already in role {role}");
                }
            }

            var audit = new SecurityAudit
            {
                Event = SecurityAuditEvent.Authorization,
                UserId = userIdOfRequester,
                Details = $"User changed roles of another user of ID {userId}",
                Severity = SecurityAuditSeverity.Medium,
            };

            _logger.LogInformation("User {userId} roles are being changed to include {roles} by User {userIdOfChanger}", 
                userId, roles, userIdOfRequester);

            await _securityAuditStore.SetSecurityAudit(audit);

            await _userManager.AddToRolesAsync(user, roles);

            _logger.LogInformation("User {userId} roles are have been changed to include {roles} by User {userIdOfChanger}",
               userId, roles, userIdOfRequester);

            return NoContent();
        }

        [HttpGet("roles/users/{userId}")]
        public async Task<ActionResult> GetUserRoles()
        {
            // Logic to retrieve roles for a specific user
            return Ok();
        }

        [HttpDelete("roles/users/{userId}")]
        public async Task<ActionResult> UnAssignUserRoles()
        {
            return Ok();
        }

        [HttpGet("roles")]
        public async Task<ActionResult> GetRoles()
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("check")]
        public ActionResult CheckAuth()
        {
            return Ok();
        }
    }
}
