using Microsoft.AspNetCore.Mvc;

namespace Evidence.API.Controllers
{
    [ApiController]
    [Route("evidence/{evidenceId}/notes")]
    public class NoteController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> CreateNote()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetNoteById()
        {
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateNoteById()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNoteById()
        {
            return Ok();
        }
    }
}
