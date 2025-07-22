using Caching;
using Evidence.API.DTOs;
using Evidence.API.Mappings;
using Evidence.API.Validators;
using Evidence.Core.Services;
using Evidence.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagination.Abstractions;
using System.Security.Claims;

namespace Evidence.API.Controllers
{
    /// <summary>
    /// Contains all Evidence API endpoint's
    /// </summary>
    [ApiController]
    [Route("evidence")]
    public class EvidenceController(IEvidenceService evidenceService, IRedisService redisService, SearchEvidenceQueryValidator searchEvidenceQueryValidator) : ControllerBase
    {
        private readonly IEvidenceService _evidenceService = evidenceService;
        private readonly IRedisService _redisService = redisService;
        private readonly EvidenceMapping _evidenceMapping = new();
        private readonly SearchEvidenceQueryValidator _searchEvidenceQueryValidator = searchEvidenceQueryValidator;

        /// <summary>
        /// Create a piece of <see cref="Core.Models.Evidence"/> and returns a upload URL
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateEvidence([FromQuery] CreateEvidenceDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var evidence = _evidenceMapping.Create(dto);
            evidence.UploadedById = userId;

            var result = await _evidenceService.CreateAsync(evidence);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(new { evidenceId = evidence.Id, result.UploadUrl });
        }

        /// <summary>
        /// Hit this endpoint when the client side uploads a file ot the cloud to mark it as uploaded
        /// </summary>
        [Authorize]
        [HttpPost("{evidenceId}/upload-complete")]
        public async Task<IActionResult> EvidenceUploadComplete(string evidenceId)
        {
            var evidence = await _evidenceService.FindByIdAsync(evidenceId);
            if (evidence is null)
            {
                return NotFound();
            }
            if (evidence.IsAlreadyUploaded())
            {
                return BadRequest();
            }

            evidence.MarkAsUploaded();
            var result = await _evidenceService.UpdateAsync(evidence);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return NoContent();
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var evidence = await _evidenceService.FindByIdAsync(evidenceId);
            if (evidence is null)
            {
                return NotFound();
            }

            var result = await _evidenceService.DeleteAsync(evidence, userId);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            await _redisService.RemoveKeyAsync(evidence.Id);

            return NoContent();
        }

        [Authorize]
        [HttpGet("{evidenceId}/download")]
        public async Task<IActionResult> DownloadEvidenceAsync(string evidenceId)
        {
            var evidence = await _evidenceService.FindByIdAsync(evidenceId);
            if (evidence is null)
            {
                return NotFound();
            }

            var result = await _evidenceService.DownloadAsync(evidence);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(new { result.DownloadUrl });
        }

        [Authorize]
        [HttpPost("reference-numbers/is-taken")]
        public async Task<IActionResult> IsReferenceNumberTaken([FromBody] ReferenceNumberRequest dto)
        {
            var isTaken = await _evidenceService.IsReferenceNumberTaken(dto.ReferenceNumber);

            return Ok(new { isTaken });
        }

        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> SearchEvidenceAsync([FromQuery] SearchEvidenceQuery query)
        {
            var queryValidatorResult = _searchEvidenceQueryValidator.Execute(query);
            if (!queryValidatorResult.IsSuccessful)
            {
                return BadRequest(queryValidatorResult);
            }

            var pagedResult = await _evidenceService.SearchAsync(query);
            var dto = new PaginatedResult<EvidenceDto>
            {
                Data = pagedResult.Data.Select(x => _evidenceMapping.ToDto(x)),
                HasNextPage = pagedResult.HasNextPage,
                HasPreviousPage = pagedResult.HasPreviousPage,
                Pagination = pagedResult.Pagination,
            };

            return Ok(dto);
        }


        [Authorize]
        [HttpGet("{evidenceId}/view")]
        public async Task<IActionResult> ViewEvidenceByIdAsync(string evidenceId)
        {
            var evidence = await _evidenceService.FindByIdAsync(evidenceId);
            if (evidence is null)
            {
                return NotFound();
            }

            var result = await _evidenceService.ViewAsync(evidence);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(new { result.ViewUrl });
        }
    }
}
