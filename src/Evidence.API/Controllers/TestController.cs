using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evidence.API.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok();
        }
    }
}
