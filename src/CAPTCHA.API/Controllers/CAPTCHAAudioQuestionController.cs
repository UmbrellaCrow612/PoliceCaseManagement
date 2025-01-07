using CAPTCHA.Core.Services;
using CAPTCHA.Infrastructure.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CAPTCHA.API.Controllers
{
    [ApiController]
    [Route("captcha/audio-questions")]
    public class CAPTCHAAudioQuestionController(ICAPTCHAAudioQuestionStore store) : ControllerBase
    {
        private readonly ICAPTCHAAudioQuestionStore _store = store;

        [HttpGet]
        public async Task<ActionResult> GetQuestion()
        {
            var (audioInBytes, question) = CAPTCHAAudioQuestionService.CreateQuestion();
            await _store.AddAsync(question);

            return Ok(new {bytes = audioInBytes, id = question.Id});
        }
    }
}
