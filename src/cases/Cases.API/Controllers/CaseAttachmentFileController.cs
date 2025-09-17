using Auth;
using Cases.API.DTOs;
using Cases.API.Mappings;
using Cases.Core.Services;
using Cases.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cases.API.Controllers
{
    [Route("cases")]
    [ApiController]
    public class CaseAttachmentFileController(ICaseService caseService, ICaseFileService caseFileService) : ControllerBase
    {
        private readonly ICaseFileService _caseFileService = caseFileService;
        private readonly ICaseService _caseService = caseService;
        private readonly CaseAttachmentFileMapping _caseAttachmentFileMapping = new();


        [Authorize]
        [HttpPost("{caseId}/attachments/upload")]
        public async Task<IActionResult> UploadAttachmentForCase(string caseId, [FromBody] CaseAttachmentFileMetaData meta)
        {
            var _case = await _caseService.FindByIdAsync(caseId);
            if (_case is null) return NotFound();

            (string preSignedUrl, string fileId) = await _caseFileService.AddAsync(_case, meta);

            var response = new UploadCaseAttachmentFileResponse
            {
                UploadUrl = preSignedUrl,
                FileId = fileId
            };

            return Ok(response);
        }


        [Authorize]
        [HttpPost("/attachments/{attachmentId}/complete")]
        public async Task<IActionResult> ConfirmClientSideUploadComplete(string attachmentId)
        {
            var attachment = await _caseFileService.FindByIdAsync(attachmentId);
            if (attachment is null)
            {
                return NotFound();
            }

            var result = await _caseFileService.UploadComplete(attachment);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }


        [Authorize]
        [HttpGet("{caseId}/attachments")]
        public async Task<IActionResult> GetCaseAttachments(string caseId)
        {
            var _case = await _caseService.FindByIdAsync(caseId);
            if (_case is null) return NotFound();

            var files = await _caseFileService.GetAsync(_case);
            var dto = files.Select(x => _caseAttachmentFileMapping.ToDto(x));

            return Ok(dto);
        }

        [Authorize]
        [HttpGet("attachments/download/{attachmentId}")]
        public async Task<IActionResult> DownloadCaseAttachmentFile(string attachmentId)
        {
            var file = await _caseFileService.FindByIdAsync(attachmentId);
            if (file is null)
            {
                return NotFound();
            }

            var url = await _caseFileService.DownloadAsync(file);

            return Ok(url);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("attachments/{attachmentId}")]
        public async Task<IActionResult> DeleteCaseFileAttachments(string attachmentId)
        {
            var file = await _caseFileService.FindByIdAsync(attachmentId);
            if (file is null) return NotFound();

            var result = await _caseFileService.DeleteAsync(file);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return NoContent();
        }
    }
}
