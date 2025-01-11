using CAPTCHA.API.DTOs;
using CAPTCHA.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAPTCHA.API.Controllers
{
    [ApiController]
    [Route("captcha/carousel-games")]
    public class CAPTCHACarouselChoiceGameQuestionController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetQuestion()
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

        [HttpPost]
        public async Task<ActionResult> ValidateQuestion([FromBody] CreateValidateCaroselGameDto gameDto)
        {
            var gId = gameDto.Id;

            for (var i = 0; i < gameDto.Choices.Count; i++)
            {

            }
            return Ok();
        }
    }
}
