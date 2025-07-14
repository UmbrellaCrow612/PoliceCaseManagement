using Cases.API.DTOs;
using Cases.API.Mappings;
using Cases.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cases.API.Controllers
{
    [ApiController]
    [Route("case-actions")]
    public class CaseActionController(ICaseActionService caseActionService, ICaseAuthorizationService caseAuthorizationService, ICaseService caseService) : ControllerBase
    {
        private readonly ICaseActionService _caseActionService = caseActionService;
        private readonly CaseActionMapping _caseActionMapping = new();
        private readonly ICaseAuthorizationService _caseAuthorizationService = caseAuthorizationService;
        private readonly ICaseService _caseService = caseService;

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


        [Authorize]
        [HttpPost("{caseId}/case-actions")]
        public async Task<IActionResult> AddCaseActionToCase(string caseId, [FromBody] CreateCaseActionDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var canAdd = await _caseAuthorizationService.CanUserAddActions(userId, caseId);
            if (!canAdd) return Forbid();

            var _case = await _caseService.FindById(caseId);
            if (_case is null)
            {
                return NotFound();
            }
            var action = _caseActionMapping.Create(dto);
            action.CaseId = caseId;
            action.CreatedById = userId;

            var result = await _caseActionService.CreateAsync(_case, action);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            var returnDto = _caseActionMapping.ToDto(action);

            return Ok(returnDto);
        }

        [Authorize]
        [HttpGet("{caseId}/case-actions")]
        public async Task<IActionResult> GetCaseActionsForCaseByIdAsync(string caseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var canView = await _caseAuthorizationService.CanUserViewCaseActions(userId, caseId);
            if (!canView) return Forbid();

            var _case = await _caseService.FindById(caseId);
            if (_case is null)
            {
                return NotFound();
            }

            var actions = await _caseActionService.GetAsync(_case);
            List<CaseActionDto> dto = [.. actions.Select(x => _caseActionMapping.ToDto(x))];

            return Ok(dto);
        }
    }
}
