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
    [Route("evidence/{evidenceId}/photos")]
    public class PhotoController (IPhotoStore photoStore, IEvidenceItemStore evidenceItemStore, IMapper mapper): ControllerBase
    {
        private readonly IPhotoStore _photoStore = photoStore;
        private readonly IEvidenceItemStore _evidenceItemStore = evidenceItemStore;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        public async Task<ActionResult> CreatePhoto(string evidenceId, [FromBody] CreatePhotoDto createPhotoDto)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var photo = _mapper.Map<Photo>(createPhotoDto);

            var (Succeeded, Errors) = await _photoStore.CreatePhotoAsync(evidence, photo);
            if (!Succeeded) return BadRequest(Errors);

            return Created();
        }

        [HttpGet]
        public async Task<ActionResult> GetPhotos(string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var photos = await _photoStore.GetPhotosAsync(evidence);

            var dto = _mapper.Map<IEnumerable<PhotoDto>>(photos);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPhotoById(string id, string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var photo = await _photoStore.GetPhotoByIdAsync(evidence, id);
            if (photo is null) return NotFound("Photo not found");

            var dto = _mapper.Map<PhotoDto>(photo);

            return Ok(dto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePhotoById(string id, string evidenceId, [FromBody] UpdatePhotoDto updatePhotoDto)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var photo = await _photoStore.GetPhotoByIdAsync(evidence, id);
            if (photo is null) return NotFound("Photo not found");

            _mapper.Map(updatePhotoDto, photo);

            await _photoStore.UpdatePhotoAsync(evidence, photo);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePhotoById(string id, string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var photo = await _photoStore.GetPhotoByIdAsync(evidence, id);
            if (photo is null) return NotFound("Photo not found");

            await _photoStore.DeletePhotoAsync(evidence,photo);

            return NoContent();
        }
    }
}
