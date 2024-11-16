using Microsoft.AspNetCore.Mvc;

namespace Evidence.API.Controllers
{
    [ApiController]
    [Route("evidence/{evidenceId}/custody-logs")]
    public class CustodyLogController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> CreateCustodyLog()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustodyLogById()
        {
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCustodyLogById()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustodyLogById()
        {
            return Ok();
        }
    }
}
