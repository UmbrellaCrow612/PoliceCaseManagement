using AutoMapper;
using Evidence.API.DTOs.Create;
using Evidence.API.DTOs.Query;
using Evidence.API.DTOs.Read;
using Evidence.API.DTOs.Update;
using Evidence.Infrastructure.Data.Models;
using Evidence.Infrastructure.Data.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evidence.API.Controllers
{
    [ApiController]
    [Route("crime-scenes")]
    public class CrimeSceneController(ICrimeSceneStore crimeSceneStore, IMapper mapper) : ControllerBase
    {
        private readonly ICrimeSceneStore _crimeSceneStore = crimeSceneStore;
        private readonly IMapper _mapper = mapper;

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateCrimeScene([FromBody] CreateCrimeSceneDto createCrimeSceneDto)
        {
            var crimeScene = _mapper.Map<CrimeScene>(createCrimeSceneDto);

            (bool Succeeded, ICollection<string> Errors) = await _crimeSceneStore.CreateCrimeScene(crimeScene);
            if (!Succeeded) return BadRequest(Errors);

            return NoContent();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ICollection<CrimeSceneDto>>> SearcheCrimeScenes([FromQuery] CrimeSceneQuery query)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("{crimeSceneId}")]
        public async Task<ActionResult<CrimeSceneDto>> GetCrimeSceneById(string crimeSceneId)
        {
            var crimeScene = await _crimeSceneStore.GetCrimeSceneById(crimeSceneId);
            if (crimeScene is null) return NotFound("Crime Scene not found.");

            var dto = _mapper.Map<CrimeSceneDto>(crimeScene);

            return Ok(dto);
        }

        [Authorize]
        [HttpPatch("{crimeSceneId}")]
        public async Task<ActionResult> UpdateCrimeScene(string crimeSceneId, [FromBody] UpdateCrimeSceneDto updateCrimeSceneDto)
        {
            var crimeScene = await _crimeSceneStore.GetCrimeSceneById(crimeSceneId);
            if (crimeScene is null) return NotFound("Crime Scene not found.");

            _mapper.Map(updateCrimeSceneDto, crimeScene);

            (bool Succeeded, ICollection<string> Errors) = await _crimeSceneStore.UpdateCrimeScene(crimeScene);
            if (!Succeeded) return BadRequest(Errors);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{crimeSceneId}")]
        public async Task<ActionResult> DeleteCrimeScene(string crimeSceneId)
        {
            var crimeScene = await _crimeSceneStore.GetCrimeSceneById(crimeSceneId);
            if (crimeScene is null) return NotFound("Crime Scene not found.");

            (bool Succeeded, ICollection<string> Errors) = await _crimeSceneStore.DeleteCrimeScene(crimeScene);
            if (!Succeeded) return BadRequest(Errors);

            return NoContent();
        }
    }
}
