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
    [Route("evidence/{evidenceId}/notes")]
    public class NoteController(INoteStore noteStore, IEvidenceItemStore evidenceItemStore, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly INoteStore _noteStore = noteStore;
        private readonly IEvidenceItemStore _evidenceItemStore = evidenceItemStore;

        [HttpPost]
        public async Task<ActionResult> CreateNote(string evidenceId, [FromBody] CreateNoteDto createNoteDto)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var note = _mapper.Map<Note>(createNoteDto);

            (bool Succeeded, IEnumerable<string> Errors) = await _noteStore.CreateNote(evidence, note);
            if (!Succeeded) return BadRequest(Errors);

            return Created();
        }

        [HttpGet]
        public async Task<ActionResult> GetNotes(string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var notes = await _noteStore.GetNotes(evidence);

            var dto = _mapper.Map<IEnumerable<NoteDto>>(notes);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetNoteById(string id, string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var note = await _noteStore.GetNoteById(evidence, id);

            var dto = _mapper.Map<NoteDto>(note);

            return Ok(dto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateNoteById(string id, string evidenceId, [FromBody] UpdateNoteDto updateNoteDto)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var note = await _noteStore.GetNoteById(evidence, id);

            _mapper.Map(updateNoteDto, note);

            await _noteStore.UpdateNote(evidence, note);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNoteById(string id, string evidenceId)
        {
            var evidence = await _evidenceItemStore.GetEvidenceById(evidenceId);
            if (evidence is null) return NotFound("Evidence not found.");

            var note = await _noteStore.GetNoteById(evidence, id);

            await _noteStore.DeleteNote(evidence, note);

            return NoContent();
        }
    }
}
