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
            var (childrenAsBytes, question, parentsChildren) = CAPTCHAGridParentQuestionService.CreateQuestion();

            return Ok(new
            {
                id = question.Id,
                childrenAsBytes,
            });
        }
    }
}
