using AutoMapper;
using Evidence.API.DTOs;
using Evidence.Infrastructure.Data.Models;
using Evidence.Infrastructure.Data.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evidence.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("evidence/{evidenceId}/custody-logs")]
    public class CustodyLogController(ICustodyLogStore custodyLogStore , IEvidenceItemStore evidenceItemStore, IMapper mapper) : ControllerBase
    {
        private readonly ICustodyLogStore _custodyLogStore = custodyLogStore;
        private readonly IEvidenceItemStore _evidenceItemStore = evidenceItemStore;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        public async Task<ActionResult> CreateCustodyLog([FromBody] CreateCustodyLogDto createCustodyLogDto, string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found");

            var log = _mapper.Map<CustodyLog>(createCustodyLogDto);

            var (Succeeded, Errors) = await _custodyLogStore.CreateCustodyLog(evidence, log);
            if (!Succeeded) return BadRequest(Errors);

            return Created();
        }

        [HttpGet]
        public async Task<ActionResult> GetCustodyLogs(string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found");

            var logs = await _custodyLogStore.GetCustodyLogs(evidence);

            var dto = _mapper.Map<IEnumerable<CustodyLogDto>>(logs);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustodyLogById(string evidenceId, string id)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found");

            var log = await _custodyLogStore.GetCustodyLogById(evidence, id);
            if(log is null) return NotFound("Custody log not found.");

            var dto = _mapper.Map<CustodyLogDto>(log);

            return Ok(dto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCustodyLogById([FromBody] UpdateCustodyLogDto updateCustodyLogDto, string id, string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found");

            var log = await _custodyLogStore.GetCustodyLogById(evidence, id);
            if (log is null) return NotFound("Custody log not found.");

            _mapper.Map(updateCustodyLogDto, log);

            await _custodyLogStore.UpdateCustodyLog(evidence, log);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustodyLogById(string id, string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found");

            var log = await _custodyLogStore.GetCustodyLogById(evidence, id);
            if (log is null) return NotFound("Custody log not found.");

            await _custodyLogStore.DeleteCustodyLog(evidence, log);

            return NoContent();
        }
    }
}
