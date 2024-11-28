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
    [Route("photos")]
    public class PhotoController (IPhotoStore photoStore, IEvidenceItemStore evidenceItemStore, IMapper mapper): ControllerBase
    {
        private readonly IPhotoStore _photoStore = photoStore;
        private readonly IEvidenceItemStore _evidenceItemStore = evidenceItemStore;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        public async Task<ActionResult> CreatePhoto(string evidenceId, [FromBody] CreatePhotoDto createPhotoDto)
        {
            var photo = _mapper.Map<Photo>(createPhotoDto);

            (bool Succeeded, ICollection<string> Errors) = await _photoStore.CreatePhotoAsync(photo);
            if (!Succeeded) return BadRequest(Errors);

            return Created();
        }

        [HttpGet("{photoId}")]
        public async Task<ActionResult> GetPhotoById(string photoId)
        {
            var photo = await _photoStore.GetPhotoByIdAsync(photoId);
            if(photo is null) return NotFound();

            var dto = _mapper.Map<PhotoDto>(photo);

            return Ok(dto);
        }

        [HttpPatch("{photoId}")]
        public async Task<ActionResult> UpdatePhotoById([FromBody] UpdatePhotoDto updatePhotoDto, string photoId)
        {
            var photo = await _photoStore.GetPhotoByIdAsync(photoId);
            if (photo is null) return NotFound();

            _mapper.Map(updatePhotoDto, photo);

            (bool Succeeded, ICollection<string> Errors) = await _photoStore.UpdatePhotoAsync(photo);
            if(!Succeeded) return BadRequest(Errors);

            return NoContent();
        }

        [HttpDelete("{photoId}")]
        public async Task<ActionResult> DeletePhotoById(string photoId)
        {
            var photo = await _photoStore.GetPhotoByIdAsync(photoId);
            if (photo is null) return NotFound();

            (bool Succeeded, ICollection<string> Errors) = await _photoStore.DeletePhotoAsync(photo);
            if (!Succeeded) return BadRequest(Errors);

            return NoContent();
        }
    }
}
