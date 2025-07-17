using System.Security.Claims;
using Caching;
using Cases.API.DTOs;
using Cases.API.Mappings;
using Cases.API.Validators;
using Cases.Core.Services;
using Cases.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagination.Abstractions;

namespace Cases.API.Controllers
{
    [ApiController]
    [Route("cases")]
    public class CasesController(ICaseService caseService, CaseValidator caseValidator, SearchCasesQueryValidator searchCasesQueryValidator, 
        IRedisService redisService, ICaseAuthorizationService caseAuthorizationService, IIncidentTypeService incidentTypeService, ICaseActionService caseActionService) : ControllerBase
    {
        private readonly ICaseService _caseService = caseService;
        private readonly IncidentTypeMapping _incidentTypeMapping = new();
        private readonly CasesMapping _caseMapping = new();
        private readonly CaseValidator _caseValidator = caseValidator;
        private readonly SearchCasesQueryValidator _searchCasesQueryValidator = searchCasesQueryValidator;
        private readonly IRedisService _redisService = redisService;
        private readonly ICaseAuthorizationService _caseAuthorizationService = caseAuthorizationService;
        private readonly IIncidentTypeService _incidentTypeService = incidentTypeService;
        private readonly ICaseActionService _caseActionService = caseActionService;
        private readonly CaseActionMapping _caseActionMapping = new();
        private readonly CaseAccessListMapping _caseAccessListMapping = new();

        [Authorize]
        [HttpGet("case-numbers/{caseNumber}/is-taken")]
        public async Task<IActionResult> IsCaseNumberTaken(string caseNumber)
        {
            var isTaken = await _caseService.IsCaseNumberTaken(caseNumber);
            if (isTaken)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("{caseId}")]
        public async Task<ActionResult<CaseDto>> GetCaseById(string caseId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var canView = await _caseAuthorizationService.CanUserViewCase(userId, caseId);
            if (!canView) return Forbid();

            var cache = await _redisService.GetStringAsync<CaseDto>(caseId);
            if (cache is not null)
            {
                return Ok(cache);
            }

            var _case = await _caseService.FindById(caseId);
            if (_case is null)
            {
                return NotFound();
            }

            var dto = _caseMapping.ToDto(_case);
            await _redisService.SetStringAsync<CaseDto>(_case.Id, dto);

            return Ok(dto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CaseDto>> CreateCase([FromBody] CreateCaseDto dto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var caseToCreate = _caseMapping.Create(dto);
            caseToCreate.CreatedById = userId;

            var valResult = _caseValidator.Execute(caseToCreate);
            if (!valResult.IsSuccessful)
            {
                return BadRequest(valResult);
            }

            var result = await _caseService.CreateAsync(caseToCreate);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            var returnDto = _caseMapping.ToDto(caseToCreate);

            return Ok(returnDto);
        }
     
        [Authorize]
        [HttpPost("{caseId}/incident-types/{incidentTypeId}")]
        public async Task<IActionResult> AddIncidentTypeToCase(string caseId, string incidentTypeId)
        {
            var _case = await _caseService.FindById(caseId);
            if (_case is null)
            {
                return NotFound();
            }

            var incidentType = await _incidentTypeService.FindByIdAsync(incidentTypeId);
            if (incidentType is null)
            {
                return NotFound();
            }

            var result = await _incidentTypeService.LinkToCase(_case, incidentType);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> SearchCases([FromQuery] SearchCasesQuery query)
        {
            var valResult = _searchCasesQueryValidator.Execute(query);
            if (!valResult.IsSuccessful)
            {
                return BadRequest(valResult);
            }

            var result = await _caseService.SearchCases(query);
            var dtoResult = new PaginatedResult<CaseDto>
            {
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage,
                Data = result.Data.Select(x => _caseMapping.ToDto(x)),
                Pagination = new PaginationMetadata
                {
                    CurrentPage = result.Pagination.CurrentPage,
                    PageSize = result.Pagination.PageSize,
                    TotalPages = result.Pagination.TotalPages,
                    TotalRecords = result.Pagination.TotalRecords,
                }
            };

            return Ok(dtoResult);
        }

        [Authorize]
        [HttpGet("{caseId}/incident-types")]
        public async Task<ActionResult<List<IncidentTypeDto>>> GetCaseIncidentTypes(string caseId)
        {
            var _case = await _caseService.FindById(caseId);
            if (_case is null)
            {
                return NotFound();
            }

            var linkedIncidentTypes = await _incidentTypeService.GetAsync(_case);

            List<IncidentTypeDto> dto = [.. linkedIncidentTypes.Select(x => _incidentTypeMapping.ToDto(x))];
            return Ok(dto);
        }

        [Authorize]
        [HttpGet("{caseId}/users")]
        public async Task<IActionResult> GetUsersLinkedToCaseById(string caseId)
        {
            var _case = await _caseService.FindById(caseId);
            if (_case is null) return NotFound();

            var users = await _caseService.GetUsersAsync(_case);
            var dto = users.Select(x => _caseAccessListMapping.ToDto(x));

            return Ok(dto);
        }

        [Authorize]
        [HttpPost("{caseId}/users")]
        public async Task<IActionResult> AssignUsersToCase(string caseId, [FromBody] AssignUserToCaseDto dto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var hasPerm = await _caseAuthorizationService.CanUserAssignCaseUsers(userId, caseId);
            if (!hasPerm)
            {
                return Forbid();
            }

            var _case = await _caseService.FindById(caseId);
            if (_case is null) return NotFound();

            var result = await _caseService.AddUser(_case, dto.UserId);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{caseId}/users/{userId}")]
        public async Task<IActionResult> RemoveUserFromCaseAsync(string caseId, string userId)
        {
            var _case = await _caseService.FindById(caseId);
            if (_case is null)
            {
                return NotFound();
            }

            var result = await _caseService.RemoveUser(_case, userId);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("{caseId}/me")]
        public async Task<IActionResult> GetCasePermissionForMe(string caseId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var _case = await _caseService.FindById(caseId);
            if (_case is null)
            {
                return NotFound();
            }

            var isLinked = await _caseService.IsUserLinkedToCase(_case, userId);
            if (!isLinked)
            {
                return BadRequest();
            }

            var role = await _caseAuthorizationService.GetUserRole(_case, userId);

            return Ok(role);
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
