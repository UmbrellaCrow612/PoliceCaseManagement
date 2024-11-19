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
    [Route("evidence/{evidenceId}/lab-results")]
    public class LabResultController(LabResultStore labResultStore, EvidenceItemStore evidenceItemStore, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly ILabResultStore _labResultStore = labResultStore;
        private readonly EvidenceItemStore _evidenceItemStore = evidenceItemStore;


        [HttpPost]
        public async Task<ActionResult> CreateLabResult(string evidenceId, [FromBody] CreateLabResultDto createLabResultDto)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if(evidence is null) return NotFound("Evidence not found.");

            var labResult = _mapper.Map<LabResult>(createLabResultDto);

            var (Succeeded, Errors) = await _labResultStore.CreateLabResult(evidence, labResult);
            if (!Succeeded) return BadRequest(Errors);

            return Created();
        }

        [HttpGet]
        public async Task<ActionResult> GetLabResults(string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var results = await _labResultStore.GetLabResults(evidence);

            var dto = _mapper.Map<IEnumerable<LabResultDto>>(results);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetLabResultById(string evidenceId, string id)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var result = await _labResultStore.GetLabResultById(evidence, id);

            var dto = _mapper.Map<LabResultDto>(result);

            return Ok(dto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateLabResultById(string id, string evidenceId, [FromBody] UpdateLabResultDto updateLabResultDto)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var labResult = await _labResultStore.GetLabResultById(evidence, id);
            if (labResult is null) return NotFound("Lab result not found.");

            _mapper.Map(updateLabResultDto, labResult);

            await _labResultStore.UpdateLabResult(evidence, labResult);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLabResultById(string id, string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var labResult = await _labResultStore.GetLabResultById(evidence, id);
            if (labResult is null) return NotFound("Lab result not found.");

            await _labResultStore.DeleteLabResult(evidence, labResult);

            return NoContent();
        }
    }
}
