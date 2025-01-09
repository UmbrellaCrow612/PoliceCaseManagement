using CAPTCHA.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAPTCHA.API.Controllers
{
    [ApiController]
    [Route("captcha/carousel-games")]
    public class CAPTCHACarouselChoiceGameQuestionController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetQuestion()
        {
            var (question, choices, answerSubChildren) = CarouselChoiceGameQuestionService.CreateQuestion();

            var choicesDto = choices.Select(x =>
            {
                return new
                {
                    x.Id,
                    answer = x.GetTempAnswer(),
                    choices = x.GetTempFullSubChoices().Select(x =>
                {
                    return new { x.Id, bytes = x.GetTempBytes() };
                })
                };
            });

            var dto = new
            {
                id = question.Id,
                choices = choicesDto,
            };

            return Ok(dto);
        }
    }
}
