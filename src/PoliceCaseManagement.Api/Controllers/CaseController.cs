using Microsoft.AspNetCore.Mvc;
using PoliceCaseManagement.Application.DTOs.Cases;
using PoliceCaseManagement.Application.Interfaces;

namespace PoliceCaseManagement.Api.Controllers
{
    [ApiController]
    [Route("cases")]
    public class CaseController(ICaseService caseService) : ControllerBase
    {
        private readonly ICaseService _caseService = caseService;

        [HttpGet("{id}")]
        public async Task<ActionResult<CaseDto>> GetCaseById(string id)
        {
            var caseToGet = await _caseService.GetCaseByIdAsync(id);
            if(caseToGet is null) return NotFound("Case not found.");

            return caseToGet;
        }

        [HttpPost]
        public async Task<ActionResult<CaseDto>> CreateCase([FromBody] CreateCaseDto request)
        {
            string userId = "";

            var createdCase = await _caseService.CreateCaseAsync(userId, request);

            return createdCase;
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCase(string id, [FromBody] UpdateCaseDto request)
        {
            var succeeded = await _caseService.UpdateCaseAsync(id, request);
            if (!succeeded) return NotFound("Case not found.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCase(string id)
        {
            var succeeded = await _caseService.DeleteCaseAsync(id);
            if (!succeeded) return NotFound("Case not found ");

            return NoContent();
        }
    }
}
