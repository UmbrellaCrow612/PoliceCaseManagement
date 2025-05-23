﻿using System.Security.Claims;
using Authorization;
using Caching;
using Cases.API.DTOs;
using Cases.API.Mappings;
using Cases.API.Validators;
using Cases.Core.Models;
using Cases.Core.Services;
using Cases.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cases.API.Controllers
{
    [ApiController]
    [Route("cases")]
    public class CasesController(ICaseService caseService, CaseValidator caseValidator, SearchCasesQueryValidator searchCasesQueryValidator, IRedisService redisService) : ControllerBase
    {
        private readonly ICaseService _caseService = caseService;
        private readonly IncidentTypeMapping _incidentTypeMapping = new();
        private readonly CasesMapping _caseMapping = new();
        private readonly CaseValidator _caseValidator = caseValidator;
        private readonly SearchCasesQueryValidator _searchCasesQueryValidator = searchCasesQueryValidator;
        private readonly IRedisService _redisService = redisService;
        private readonly CaseActionMapping _caseActionMapping = new();
        private readonly CaseUserMapping _caseUserMapping = new();


        private static readonly string _incidentTypesKey = "incident_types_key";


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
            var caseToCreate = _caseMapping.Create(dto);
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

        /// <summary>
        /// Link a case to a incident type by there ID's
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="incidentTypeId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("{caseId}/incident-types/{incidentTypeId}")]
        public async Task<IActionResult> AddIncidentTypeToCase(string caseId, string incidentTypeId)
        {
            var _case = await _caseService.FindById(caseId);
            if (_case is null)
            {
                return NotFound();
            }
            var incidentType = await _caseService.FindIncidentTypeById(incidentTypeId);
            if (incidentType is null)
            {
                return NotFound();
            }

            var result = await _caseService.AddToIncidentType(_case, incidentType);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok();
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpPost("incident-types")]
        public async Task<ActionResult<IncidentTypeDto>> CreateIncidentType([FromBody] CreateIncidentTypeDto dto)
        {
            var incidentType = _incidentTypeMapping.Create(dto);
            var result = await _caseService.CreateIncidentType(incidentType);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            var returnDto = _incidentTypeMapping.ToDto(incidentType);
            await _redisService.RemoveKeyAsync(_incidentTypesKey);

            return Ok(returnDto);
        }

        /// <summary>
        /// Get all incident types a case can be linked to.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("incident-types")]
        public async Task<IActionResult> GetAllCaseIncidentTypes()
        {
            var value = await _redisService.GetStringAsync<List<IncidentTypeDto>>(_incidentTypesKey);
            if (value is not null)
            {
                return Ok(value);
            }

            var incidentTypes = await _caseService.GetAllIncidentTypes();
            List<IncidentTypeDto> dto = [];
            foreach (var incidentType in incidentTypes)
            {
                dto.Add(_incidentTypeMapping.ToDto(incidentType));
            }
            await _redisService.SetStringAsync<List<IncidentTypeDto>>(_incidentTypesKey, dto);

            return Ok(dto);
        }

        /// <summary>
        /// Get details about a incident type by there ID
        /// </summary>
        [Authorize]
        [HttpGet("incident-types/{incidentTypeId}")]
        public async Task<IActionResult> GetIncidentTypeDetailsById(string incidentTypeId)
        {
            var incidentType = await _caseService.FindIncidentTypeById(incidentTypeId);
            if (incidentType is null)
            {
                return NotFound();
            }

            var dto = _incidentTypeMapping.ToDto(incidentType);

            return Ok(dto);
        }

        /// <summary>
        /// Get the amount of time a incident type is linked to how many cases.
        /// </summary>
        /// <returns>The count as a number</returns>
        [Authorize]
        [HttpGet("incident-types/{incidentTypeId}/case-incidents/count")]
        public async Task<IActionResult> GetCaseIncidentTypeCount(string incidentTypeId)
        {
            var incidentType = await _caseService.FindIncidentTypeById(incidentTypeId);
            if (incidentType is null)
            {
                return NotFound();
            }

            var count = await _caseService.GetCaseIncidentCount(incidentType);

            return Ok(new { count });
        }


        /// <summary>
        /// Admin can delete a incident type - unlinks it from cases and the incident type itself is deleted.
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("incident-types/{incidentTypeId}")]
        public async Task<IActionResult> DeleteIncidentType(string incidentTypeId)
        {
            var incidentType = await _caseService.FindIncidentTypeById(incidentTypeId);
            if (incidentType is null)
            {
                return NotFound();
            }

            var result = await _caseService.DeleteIncidentType(incidentType);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        /// <summary>
        /// Update a incident type - only admins can do this.
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("incident-types/{incidentTypeId}")]
        public async Task<IActionResult> UpdateIncidentType(string incidentTypeId, [FromBody] UpdateIncidentTypeDto dto)
        {
            var incidentType = await _caseService.FindIncidentTypeById(incidentTypeId);
            if (incidentType is null)
            {
                return NotFound();
            }

            _incidentTypeMapping.Update(incidentType, dto);

            var result = await _caseService.UpdateIncidentType(incidentType);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            await _redisService.RemoveKeyAsync(_incidentTypesKey);

            return NoContent();
        }

        /// <summary>
        /// Update a cases linked incident types - unlinks currently linked ones and links the new ones passed
        /// </summary>
        [Authorize]
        [HttpPut("{caseId}/incident-types")]
        public async Task<IActionResult> UpdateCasesLinkedIncidentTypes(string caseId, [FromBody] UpdateCasesLinkedIncidentTypesDto dto)
        {
            var _case = await _caseService.FindById(caseId);
            if(_case is null)
            {
                return NotFound("Case not found");
            }

            List<IncidentType> incidentTypes = [];

            foreach (var incidentTypeId in dto.IncidentTypeIds)
            {
                var incidentType = await _caseService.FindIncidentTypeById(incidentTypeId);
                if (incidentType is null)
                {
                    return NotFound("Incident type not found");
                }
                incidentTypes.Add(incidentType);
            }

            var result = await _caseService.UpdateCaseLinkedIncidentTypes(_case, incidentTypes);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
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
            var dtoResult = new PagedResult<CaseDto>(result.Items.Select(x => _caseMapping.ToDto(x)).ToList(), result.TotalCount, result.PageNumber, result.PageSize);

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

            var linkedIncidentTypes = await _caseService.GetIncidentTypes(_case);

            List<IncidentTypeDto> dto = [.. linkedIncidentTypes.Select(x => _incidentTypeMapping.ToDto(x))];
            return Ok(dto);
        }

        /// <summary>
        /// Get all case actions for a given case by it's ID
        /// </summary>
        [Authorize]
        [HttpGet("{caseId}/case-actions")]
        public async Task<IActionResult> GetCaseActionsForCaseByIdAsync(string caseId)
        {
            var _case = await _caseService.FindById(caseId);
            if (_case is null)
            {
                return NotFound();
            }

            var actions = await _caseService.GetCaseActions(_case);
            List<CaseActionDto> dto = [.. actions.Select(x => _caseActionMapping.ToDto(x))];

            return Ok(dto);
        }

        /// <summary>
        /// Add a case action to a given case by there id
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("{caseId}/case-actions")]
        public async Task<IActionResult> AddCaseActionToCase(string caseId, [FromBody] CreateCaseActionDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var _case = await _caseService.FindById(caseId);
            if (_case is null)
            {
                return NotFound();
            }
            var action = _caseActionMapping.Create(dto);
            action.CaseId = caseId;
            action.CreatedById = userId;

            var result = await _caseService.AddCaseAction(_case, action);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            var returnDto = _caseActionMapping.ToDto(action);

            return Ok(returnDto);
        }

        /// <summary>
        /// Get all users stored who are linked to a given case
        /// </summary>
        [Authorize]
        [HttpGet("{caseId}/users")]
        public async Task<IActionResult> GetUsersLinkedToCaseById(string caseId)
        {
            var _case = await _caseService.FindById(caseId);
            if (_case is null) return NotFound();

            var users = await _caseService.GetCaseUsers(_case);
            List<CaseUserDto> dto = [.. users.Select(x => _caseUserMapping.ToDto(x))];

            return Ok(dto);
        }

        /// <summary>
        /// Assign a set of users to a case
        /// </summary>
        [Authorize]
        [HttpPost("{caseId}/users")]
        public async Task<IActionResult> AssignUsersToCase(string caseId, [FromBody] AssignUsersToCaseDto dto)
        {
            var _case = await _caseService.FindById(caseId);
            if (_case is null) return NotFound();

            var result = await _caseService.AddUsers(_case, dto.UserIds);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }
    }
}
