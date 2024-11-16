using AutoMapper;
using Evidence.API.DTOs;
using Evidence.Infrastructure.Data.Models;
using Evidence.Infrastructure.Data.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Evidence.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("evidence")]
    public class EvidenceController(IEvidenceItemStore evidenceItemStore, IMapper mapper) : ControllerBase
    {
        private readonly IEvidenceItemStore _evidenceItemStore = evidenceItemStore;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        public async Task<ActionResult> CreateEvidence([FromBody] CreateEvidenceItemDto createEvidenceDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized("User ID not found on JWT");

            var evidence = _mapper.Map<EvidenceItem>(createEvidenceDto);
            evidence.CreatedById = userId;

            var (Succeeded, Errors) = await _evidenceItemStore.CreateEvidence(evidence);

            if (!Succeeded)
            {
                return BadRequest(Errors);
            }

            return Ok(new { id = evidence.Id });
        }
    }
}
