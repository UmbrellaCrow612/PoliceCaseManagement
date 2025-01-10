using AutoMapper;
using Evidence.API.DTOs.Create;
using Evidence.API.DTOs.Read;
using Evidence.API.DTOs.Update;
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

            var (Succeeded, Errors) = await _evidenceItemStore.CreateEvidenceAsync(evidence);

            if (!Succeeded)
            {
                return BadRequest(Errors);
            }

            return Created(nameof(CreateEvidence), new { id = evidence.Id });
        }

        [HttpGet("{evidenceId}")]
        public async Task<ActionResult> GetEvidenceById(string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceByIdAsync(evidenceId);
            if (evidence is null) return NotFound();

            var dto = _mapper.Map<EvidenceItemDto>(evidence);

            return Ok(dto);
        }

        [HttpPatch("{evidenceId}")]
        public async Task<ActionResult> PatchEvidenceById(string evidenceId, [FromBody] UpdateEvidenceItemDto updateEvidenceItemDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized("User ID not found on JWT");

            var evidence = await _evidenceItemStore.GetEvidenceByIdAsync(evidenceId);
            if (evidence is null) return NotFound();

            _mapper.Map(updateEvidenceItemDto, evidence);

            (bool Succeeded,ICollection<string> Errors) = await _evidenceItemStore.UpdateEvidenceAsync(userId, evidence);
            if(!Succeeded) return BadRequest(Errors);

            return NoContent();
        }

        [HttpDelete("{evidenceId}")]
        public async Task<ActionResult> DeleteEvidenceById(string evidenceId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized("User ID not found on JWT");

            var evidence = await _evidenceItemStore.GetEvidenceByIdAsync(evidenceId);
            if (evidence is null) return NotFound();

            (bool Succeeded,ICollection<string> Errors) = await _evidenceItemStore.DeleteEvidenceAsync(userId, evidence);
            if(!Succeeded) return BadRequest(Errors);

            return NoContent();
        }
    }
}
