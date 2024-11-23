using Identity.Infrastructure.Data.Models;
using Identity.Infrastructure.Data.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("authorization")]
    public class AuthorizationController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<AuthorizationController> logger, ISecurityAuditStore securityAuditStore) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ILogger<AuthorizationController> _logger = logger;
        private readonly ISecurityAuditStore _securityAuditStore = securityAuditStore;

        [Authorize]
        [HttpPost("roles/users/{userId}")]
        public async Task<ActionResult> AssignUserRoles([FromBody] IEnumerable<string> roles, string userId)
        {
            var userIdOfRequester = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdOfRequester)) return BadRequest("Jwt Name Identifier is null.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound("User not found.");

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

        [Authorize]
        [HttpGet("roles/users/{userId}")]
        public async Task<ActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound("User not found.");

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }

        [Authorize]
        [HttpDelete("roles/users/{userId}")]
        public async Task<ActionResult> UnAssignUserRoles([FromBody] IEnumerable<string> roles, string userId)
        {
            var userIdOfRequester = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdOfRequester)) return BadRequest("Jwt Name Identifier is null.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound("User not found.");

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return NotFound($"{role} dose not exist.");
                }

                if (!await _userManager.IsInRoleAsync(user, role))
                {
                    return BadRequest($"User is not in role {role}");
                }
            }

            var audit = new SecurityAudit
            {
                Event = SecurityAuditEvent.Authorization,
                UserId = userIdOfRequester,
                Details = $"User changed roles of another user of ID {userId}",
                Severity = SecurityAuditSeverity.Medium,
            };

            await _securityAuditStore.SetSecurityAudit(audit);

            await _userManager.RemoveFromRolesAsync(user, roles);

            _logger.LogInformation("User {userId} roles are have been changed to exclude {roles} by User {userIdOfChanger}",
                                    userId, roles, userIdOfRequester);

            return NoContent();
        }

        [HttpGet("roles")]
        public async Task<ActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return Ok(roles);
        }

        [Authorize]
        [HttpGet("check")]
        public ActionResult CheckAuth()
        {
            return Ok();
        }
    }
}
