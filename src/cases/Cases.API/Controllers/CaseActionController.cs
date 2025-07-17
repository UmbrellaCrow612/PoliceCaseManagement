using Cases.API.Mappings;
using Cases.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cases.API.Controllers
{
    [ApiController]
    [Route("case-actions")]
    public class CaseActionController(ICaseActionService caseActionService) : ControllerBase
    {
        private readonly ICaseActionService _caseActionService = caseActionService;
        private readonly CaseActionMapping _caseActionMapping = new();

        [Authorize]
        [HttpGet("{caseActionId}")]
        public async Task<IActionResult> GetCaseActionByIdAsync(string caseActionId)
        {
            var action = await _caseActionService.FindByIdAsync(caseActionId);
            if (action is null)
            {
                return NotFound();
            }

            var dto = _caseActionMapping.ToDto(action);

            return Ok(dto);
        }
    }
}
