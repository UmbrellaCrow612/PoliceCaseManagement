using Authorization;
using Cases.API.DTOs;
using Cases.API.Mappings;
using Cases.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cases.API.Controllers
{
    [ApiController]
    [Route("cases")]
    public class CasesController(ICaseService caseService) : ControllerBase
    {
        private readonly ICaseService _caseService = caseService;
        private readonly IncidentTypeMapping _incidentTypeMapping = new();
        private readonly CasesMapping _caseMapping = new();

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CaseDto>> CreateCase([FromBody] CreateCaseDto dto)
        {
            var caseToCreate = _caseMapping.Create(dto);
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
    }
}
