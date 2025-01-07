using CAPTCHA.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAPTCHA.API.Controllers
{
    [ApiController]
    [Route("captcha/grid")]
    public class CAPTCHAGridController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetQuestion()
        {
            var (question, parentsChildren) = CAPTCHAGridParentQuestionService.CreateQuestion();

            var children = parentsChildren.Select(x => new { x.Id, Bytes = x.GetTempBytes() });

            return Ok(new
            {
                id = question.Id,
                children,
            });
        }
    }
}
