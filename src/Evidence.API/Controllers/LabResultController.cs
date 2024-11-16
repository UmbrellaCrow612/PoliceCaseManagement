using Microsoft.AspNetCore.Mvc;

namespace Evidence.API.Controllers
{
    [ApiController]
    [Route("evidence/{evidenceId}/lab-results")]
    public class LabResultController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> CreateLabResult()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetLabResultById()
        {
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateLabResultById()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLabResultById()
        {
            return Ok();
        }
    }
}
