using Caching;
using Evidence.API.DTOs;
using Evidence.API.Mappings;
using Evidence.API.Validators;
using Evidence.Core.Services;
using Evidence.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evidence.API.Controllers
{
    /// <summary>
    /// Contains all Evidence API endpoint's
    /// </summary>
    [ApiController]
    [Route("evidence")]
    public class EvidenceController(IEvidenceService evidenceService, IRedisService redisService, ITagService tagService, SearchTagsQueryValidator searchTagsQueryValidator) : ControllerBase
    {
        private readonly IEvidenceService _evidenceService = evidenceService;
        private readonly IRedisService _redisService = redisService;
        private readonly EvidenceMapping _evidenceMapping = new();
        private readonly ITagService _tagService = tagService;
        private readonly SearchTagsQueryValidator _searchTagsQueryValidator = searchTagsQueryValidator;
        private readonly TagMapping _tagMapping = new();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEvidence([FromBody] CreateEvidenceDto dto)
        {
            var evidence = _evidenceMapping.Create(dto);

            // todo do it like case attachment file uplads suing client side URLS
            var result = await _evidenceService.CreateAsync(evidence);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            var returnDto = _evidenceMapping.ToDto(evidence);

            await _redisService.SetStringAsync(evidence.Id, returnDto);

            return Ok(returnDto);
        }

        [Authorize]
        [HttpGet("{evidenceId}")]
        public async Task<IActionResult> GetEvidenceByIdAsync(string evidenceId)
        {
            var cache = await _redisService.GetStringAsync<EvidenceDto>(evidenceId);
            if (cache is not null)
            {
                return Ok(cache);
            }

            var evidence = await _evidenceService.FindByIdAsync(evidenceId);
            if (evidence is null)
            {
                return NotFound();
            }

            var dto = _evidenceMapping.ToDto(evidence);

            await _redisService.SetStringAsync<EvidenceDto>(evidence.Id, dto);

            return Ok(dto);
        }

        [Authorize]
        [HttpPatch("{evidenceId}")]
        public async Task<IActionResult> UpdateEvidenceByIdAsync(string evidenceId, [FromBody] UpdateEvidenceDto dto)
        {
            var evidence = await _evidenceService.FindByIdAsync(evidenceId);
            if (evidence is null)
            {
                return NotFound();
            }

            _evidenceMapping.Update(evidence, dto);

            var result = await _evidenceService.UpdateAsync(evidence);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            await _redisService.RemoveKeyAsync(evidence.Id);
            var cacheEntry = _evidenceMapping.ToDto(evidence);
            await _redisService.SetStringAsync<EvidenceDto>(evidence.Id, cacheEntry);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{evidenceId}")]
        public async Task<IActionResult> DeleteEvidenceByIdAsync(string evidenceId)
        {
            var evidence = await _evidenceService.FindByIdAsync(evidenceId);
            if (evidence is null)
            {
                return NotFound();
            }

            var result = await _evidenceService.DeleteAsync(evidence);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            await _redisService.RemoveKeyAsync(evidence.Id);

            return NoContent();
        }

        /// <summary>
        /// Check if a <see cref="Core.Models.Evidence.ReferenceNumber"/> is taken by another
        /// </summary>
        [Authorize]
        [HttpPost("reference-numbers/is-taken")]
        public async Task<IActionResult> IsReferenceNumberTaken([FromBody] ReferenceNumberRequest dto)
        {
            var isTaken = await _evidenceService.IsReferenceNumberTaken(dto.ReferenceNumber);

            return Ok(new { isTaken });
        }


        [Authorize]
        [HttpGet("tags")]
        public async Task<IActionResult> SearchTags([FromQuery] SearchTagsQuery query)
        {
            var validationResult = _searchTagsQueryValidator.Execute(query);
            if (!validationResult.IsSuccessful)
            {
                return BadRequest(validationResult);
            }

            var tags = await _tagService.SearchAsync(query);

            return Ok(tags);
        }
    }
}
