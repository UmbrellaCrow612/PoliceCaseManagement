using Identity.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("securityAudits")]
    public class SecurityAuditController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> SearchSecurityAudits([FromQuery] SearchSecurityAuditQuery query)
        {
            return Ok();
        }
    }
}
