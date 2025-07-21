using Evidence.API.Validators;
using Evidence.Core.Services;
using Evidence.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evidence.API.Controllers
{
    [ApiController]
    [Route("tags")]
    public class TagController(ITagService tagService, SearchTagsQueryValidator searchTagsQueryValidator) : ControllerBase
    {
        private readonly SearchTagsQueryValidator _searchTagsQueryValidator = searchTagsQueryValidator;
        private readonly ITagService _tagService = tagService;

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> SearchTags([FromQuery] SearchTagsQuery query)
        {
            var validationResult = _searchTagsQueryValidator.Execute(query);
            if (!validationResult.IsSuccessful)
            {
                return BadRequest(validationResult);
            }

            var tags = await _tagService.SearchAsync(query);
            // todo use dto as serilzation will fail
            return Ok();
        }
    }
}
