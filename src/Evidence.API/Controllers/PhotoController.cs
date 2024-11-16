using Microsoft.AspNetCore.Mvc;

namespace Evidence.API.Controllers
{
    [ApiController]
    [Route("evidence/{evidenceId}/photos")]
    public class PhotoController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult> CreatePhoto()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPhotoById()
        {
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePhotoById()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePhotoById()
        {
            return Ok();
        }
    }
}
