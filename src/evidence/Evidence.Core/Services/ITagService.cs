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
        Task<IResult> CreateAsync(Tag tag);

        /// <summary>
        /// Search a list of <see cref="Tag"/>'s in the system based on <see cref="SearchTagsQuery"/>
        /// </summary>
        /// <param name="query">Query object</param>
        /// <returns>List of matching tags</returns>
        Task<PaginatedResult<Tag>> SearchAsync(SearchTagsQuery query);
    }
}
