using Identity.API.Helpers;
using Identity.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController(JwtHelper jwtHelper) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = new ApplicationUser
            {
                UserName = "12",
                Email = "12@gmail.com"
            };

            IList<string> roles = ["12", "12"];

            var token = jwtHelper.GenerateToken(user, roles);

            return Ok(token);
        }

        [Authorize]
        [HttpGet("sec")]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
