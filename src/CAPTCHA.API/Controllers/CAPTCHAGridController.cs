using CAPTCHA.API.DTOs;
using CAPTCHA.Core.Services;
using CAPTCHA.Infrastructure.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CAPTCHA.API.Controllers
{
    [ApiController]
    [Route("captcha/grid")]
    public class CAPTCHAGridController(ICAPTCHAGridQuestionStore store, ICAPTCHAGridChildStore childStore) : ControllerBase
    {
        private readonly ICAPTCHAGridQuestionStore _gridStore = store;
        private readonly ICAPTCHAGridChildStore _childStore = childStore;

        [HttpGet]
        public async Task<ActionResult> GetQuestion()
        {
            var (question, parentsChildren, fullChildren) = CAPTCHAGridParentQuestionService.CreateQuestion();
            await _gridStore.AddAsync(question);
            await _childStore.AddManyAsync(parentsChildren);

            var children = fullChildren.Select(x => new { x.Id, Bytes = x.GetTempBytes() });

            return Ok(new
            {
                id = question.Id,
                children,
            });
        }

        [HttpPost]
        public async Task<ActionResult> ValidateQuestion([FromBody] CreateGridQuestionCAPTCHADto dto)
        {
            var question = await _gridStore.FindByIdAsync(dto.Id);
            if(question is null) return NotFound("Question not found.");

            if (!question.IsValid(dto.SelectedIds)) return BadRequest("Question invalid or slected id's do not match.");

            return Ok();
        }
    }
}
