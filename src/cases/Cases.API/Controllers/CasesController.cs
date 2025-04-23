using Authorization;
using Cases.API.DTOs;
using Cases.API.Mappings;
using Cases.API.Validators;
using Cases.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cases.API.Controllers
{
    [ApiController]
    [Route("cases")]
    public class CasesController(ICaseService caseService, CaseValidator caseValidator) : ControllerBase
    {
        private readonly ICaseService _caseService = caseService;
        private readonly IncidentTypeMapping _incidentTypeMapping = new();
        private readonly CasesMapping _caseMapping = new();
        private readonly CaseValidator _caseValidator = caseValidator;

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CaseDto>> CreateCase([FromBody] CreateCaseDto dto)
        {
            var caseToCreate = _caseMapping.Create(dto);
            var valResult = _caseValidator.Execute(caseToCreate);
            if (!valResult.IsSuccessful)
            {
                return BadRequest(valResult.Errors);
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
            var incidentTypes = await _caseService.GetAllIncidentTypes();
            List<IncidentTypeDto> dto = [];
            foreach (var incidentType in incidentTypes)
            {
                dto.Add(_incidentTypeMapping.ToDto(incidentType));
            }
            return Ok(dto);
        }

        /// <summary>
        /// Get details about a incident type bby there ID
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
        [HttpPatch("incident-types/{incidentTypeId}")]
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

            return NoContent();
        }
    }
}
