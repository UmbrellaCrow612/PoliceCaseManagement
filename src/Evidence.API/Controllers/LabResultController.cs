using AutoMapper;
using Evidence.API.DTOs.Create;
using Evidence.API.DTOs.Read;
using Evidence.API.DTOs.Update;
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
            var evidence = await _evidenceItemStore.GetEvidenceByIdAsync(evidenceId);
            if(evidence is null) return NotFound("Evidence not found.");

            var labResult = _mapper.Map<LabResult>(createLabResultDto);
            labResult.EvidenceItemId = evidenceId;

            (bool Succeeded,ICollection<string> Errors) = await _labResultStore.CreateLabResultAsync(evidence, labResult);
            if (!Succeeded) return BadRequest(Errors);

            return Created();
        }

        [HttpGet]
        public async Task<ActionResult> GetLabResults(string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceByIdAsync(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var results = await _labResultStore.GetLabResultsAsync(evidence);

            var dto = _mapper.Map<IEnumerable<LabResultDto>>(results);

            return Ok(dto);
        }

        [HttpGet("{labResultId}")]
        public async Task<ActionResult> GetLabResultById(string evidenceId, string labResultId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceByIdAsync(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var result = await _labResultStore.GetLabResultByIdAsync(evidence, labResultId);
            if (result is null) return NotFound("Lab Result not found.");

            var dto = _mapper.Map<LabResultDto>(result);

            return Ok(dto);
        }

        [HttpPatch("{labResultId}")]
        public async Task<ActionResult> UpdateLabResultById(string labResultId, string evidenceId, [FromBody] UpdateLabResultDto updateLabResultDto)
        {
            var evidence = await _evidenceItemStore.GetEvidenceByIdAsync(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var labResult = await _labResultStore.GetLabResultByIdAsync(evidence, labResultId);
            if (labResult is null) return NotFound("Lab result not found.");

            _mapper.Map(updateLabResultDto, labResult);

            (bool Succeeded,ICollection<string> Errors) = await _labResultStore.UpdateLabResultAsync(evidence, labResult);
            if (!Succeeded) return BadRequest(Errors);

            return NoContent();
        }

        [HttpDelete("{labResultId}")]
        public async Task<ActionResult> DeleteLabResultById(string labResultId, string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceByIdAsync(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var labResult = await _labResultStore.GetLabResultByIdAsync(evidence, labResultId);
            if (labResult is null) return NotFound("Lab result not found.");

            (bool Succeeded,ICollection<string> Errors) = await _labResultStore.DeleteLabResultAsync(evidence, labResult);
            if(!Succeeded) return BadRequest(Errors);

            return NoContent();
        }
    }
}
