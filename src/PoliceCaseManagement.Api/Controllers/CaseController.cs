using Microsoft.AspNetCore.Mvc;
using PoliceCaseManagement.Api.Exceptions;
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

            return caseToGet is null ? NotFound() : caseToGet;
        }

        [HttpPost]
        public async Task<ActionResult<CaseDto>> CreateCase([FromBody] CreateCaseDto request)
        {
            string userId = "1";

            var createdCase = await _caseService.CreateCaseAsync(userId, request);

            return createdCase;
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCase(string id, [FromBody] UpdateCaseDto request)
        {
            var succeeded = await _caseService.UpdateCaseAsync(id, request);

            return succeeded ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCase(string id)
        {
            string userId = "1";

            var succeeded = await _caseService.DeleteCaseAsync(id, userId);

            return succeeded ? NoContent() : NotFound();
        }
    }
}
