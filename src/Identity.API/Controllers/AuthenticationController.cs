using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Login()
        {
            return Ok();
        }
    }
}
