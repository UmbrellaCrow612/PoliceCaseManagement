using Evidence.Core.Models;
using Evidence.Core.ValueObjects;
using Pagination.Abstractions;
using Results.Abstractions;

namespace Evidence.Core.Services
{
    /// <summary>
    /// Contains all business logic to interact with <see cref="Models.Tag"/>
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// Create a <see cref="Tag"/> into the system
        /// </summary>
        /// <param name="tag">The tag object to create</param>
        /// <returns>Result object operation</returns>
        Task<TagServiceResult> CreateAsync(Tag tag);

        /// <summary>
        /// Search a list of <see cref="Tag"/>'s in the system based on <see cref="SearchTagsQuery"/>
        /// </summary>
        /// <param name="query">Query object</param>
        /// <returns>List of matching tags</returns>
        Task<PaginatedResult<Tag>> SearchAsync(SearchTagsQuery query);
    }

    public class TagServiceResult : IResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<IResultError> Errors { get; set; } = [];

        public void AddError(string code, string? message)
        {
            Errors.Add(new TagServiceError { Code = code, Message = message });
        }
    }

    public class TagServiceError : IResultError
    {
        public required string Code { get; set; }
        public required string? Message { get; set; }
    }
}
