using CAPTCHA.API.DTOs;
using CAPTCHA.Core.Services;
using CAPTCHA.Infrastructure.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CAPTCHA.API.Controllers
{
    [ApiController]
    [Route("captcha/math-questions")]
    public class CAPTCHAMathQuestionController(ICAPTCHAMathQuestionStore mathQuestionStore) : ControllerBase
    {
        private readonly ICAPTCHAMathQuestionStore _mathQuestionStore = mathQuestionStore;

        [HttpGet]
        public async Task<ActionResult> GetQuestion()
        {
            var (imgQuestionBytes, question) = CAPTCHAMathService.CreateQuestion();
            await _mathQuestionStore.AddAsync(question);

            return Ok(new { bytes = imgQuestionBytes, id = question.Id });
        }

        [HttpPost]
        public async Task<ActionResult> ValidateQuestion([FromBody] CreateMathQuestionCAPTCHADto dto)
        {
            var userAgent = Request.Headers.UserAgent.ToString();
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var convertedSuc = double.TryParse(dto.Answer, out var answer);
            if (!convertedSuc) return BadRequest("Answer must be a double precsion number.");

            var question = await _mathQuestionStore.FindByIdAsync(dto.MathQuestionId);
            if(question is null) return NotFound("Question not found.");

            if (question.UserEntriesExists())
            {
                if (question.Suspicious(userAgent, ipAddress ?? "unknown"))
                {
                    return BadRequest("Diffrent user agent and Ip Adress being used.");
                }
            }

            if (question.MaxAttemptLimitReached()) return BadRequest("Question answer limit reached.");
            question.IncrementAttempts();

            if (!question.IsValid())
            {
                return BadRequest("Question no longer valid.");
            }

            question.SetUser(userAgent, ipAddress ?? "unknown");
            if (!question.CheckAnswer(answer.ToString()))
            {
                await _mathQuestionStore.UpdateAsync(question);
                return BadRequest("AnswerHash incorrect.");
            }

            question.MarkAsSuccessful();
            await _mathQuestionStore.UpdateAsync(question);

            return Ok();
        }
    }
}
