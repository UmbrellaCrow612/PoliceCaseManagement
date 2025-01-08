using CAPTCHA.API.DTOs;
using CAPTCHA.Core.Services;
using CAPTCHA.Infrastructure.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CAPTCHA.API.Controllers
{
    [ApiController]
    [Route("captcha/grid")]
    public class CAPTCHAGridQuestionController(ICAPTCHAGridQuestionStore store) : ControllerBase
    {
        private readonly ICAPTCHAGridQuestionStore _gridStore = store;

        [HttpGet]
        public async Task<ActionResult> GetQuestion()
        {
            var (questionText, question, answerChildren, fullChildren) = GridQuestionService.CreateQuestion();
            await _gridStore.AddAsync(question, answerChildren);

            var children = fullChildren.Select(x => new { x.Id, Bytes = x.GetTempBytes() });

            return Ok(new
            {
                id = question.Id,
                text = questionText,
                children,
            });
        }

        [HttpPost]
        public async Task<ActionResult> ValidateQuestion([FromBody] CreateGridQuestionCAPTCHADto dto)
        {
            var question = await _gridStore.FindByIdAsync(dto.Id);
            if(question is null) return NotFound("Question not found.");

            var (isValid, errors) = question.IsValid(dto.SelectedIds);
            if (!isValid) return BadRequest(new { errors });

            question.MarkUsed();
            await _gridStore.UpdateAsync(question);

            return Ok();
        }
    }
}
