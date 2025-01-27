using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    /// <summary>
    /// Handles JWT and other tokens the application issues the ability to search an filter tokens
    /// and revoke and monitor them, RBA with admin that can do it
    /// </summary>
    [ApiController]
    [Route("tokens")]
    public class TokenController : ControllerBase
    {
        [HttpGet("search")]
        public ActionResult SearchTokens()
        {
            return Ok();
        }

        [HttpPost("users/{userId}/revoke-all")]
        public ActionResult RevokeuserTokens()
        {
            return Ok();
        }

        [HttpGet("users/{userId}")]
        public ActionResult GetUsersTokens()
        {
            return Ok();
        }
    }
}
