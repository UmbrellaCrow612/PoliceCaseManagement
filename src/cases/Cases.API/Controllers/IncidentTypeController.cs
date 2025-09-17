using Cases.API.DTOs;
using Cases.API.Mappings;
using Cases.Core.Services;
using Cases.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagination.Abstractions;
using Auth;

namespace Cases.API.Controllers
{
    [ApiController]
    [Route("incident-types")]
    public class IncidentTypeController(IIncidentTypeService incidentTypeService) : ControllerBase
    {
        private readonly IIncidentTypeService _incidentTypeService = incidentTypeService;
        private readonly IncidentTypeMapping _incidentTypeMapping = new();

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<IncidentTypeDto>> CreateIncidentType([FromBody] CreateIncidentTypeDto dto)
        {
            var incidentType = _incidentTypeMapping.Create(dto);
            var result = await _incidentTypeService.CreateAsync(incidentType);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            var returnDto = _incidentTypeMapping.ToDto(incidentType);

            return Ok(returnDto);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SearchIncidentTypes([FromQuery] SearchIncidentTypesQuery query)
        {
            var paginatedResponse = await _incidentTypeService.SearchAsync(query);
            var dto = new PaginatedResult<IncidentTypeDto>
            {
                Data = paginatedResponse.Data.Select(x => _incidentTypeMapping.ToDto(x)),
                HasNextPage = paginatedResponse.HasNextPage,
                HasPreviousPage = paginatedResponse.HasPreviousPage,
                Pagination = paginatedResponse.Pagination,
            };

            return Ok(dto);
        }

        [Authorize]
        [HttpGet("{incidentTypeId}")]
        public async Task<IActionResult> GetIncidentTypeDetailsById(string incidentTypeId)
        {
            var incidentType = await _incidentTypeService.FindByIdAsync(incidentTypeId);
            if (incidentType is null)
            {
                return NotFound();
            }

            var dto = _incidentTypeMapping.ToDto(incidentType);

            return Ok(dto);
        }

        [Authorize]
        [HttpGet("{incidentTypeId}/case-links")]
        public async Task<IActionResult> GetCaseIncidentTypeCount(string incidentTypeId)
        {
            var incidentType = await _incidentTypeService.FindByIdAsync(incidentTypeId);
            if (incidentType is null)
            {
                return NotFound();
            }

            var count = await _incidentTypeService.CountCaseLinks(incidentType);

            return Ok(new { count });
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{incidentTypeId}")]
        public async Task<IActionResult> DeleteIncidentType(string incidentTypeId)
        {
            var incidentType = await _incidentTypeService.FindByIdAsync(incidentTypeId);
            if (incidentType is null)
            {
                return NotFound();
            }

            var result = await _incidentTypeService.DeleteAsync(incidentType);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{incidentTypeId}")]
        public async Task<IActionResult> UpdateIncidentType(string incidentTypeId, [FromBody] UpdateIncidentTypeDto dto)
        {
            var incidentType = await _incidentTypeService.FindByIdAsync(incidentTypeId);
            if (incidentType is null)
            {
                return NotFound();
            }

            _incidentTypeMapping.Update(incidentType, dto);

            var result = await _incidentTypeService.UpdateAsync(incidentType);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }

    }
}
