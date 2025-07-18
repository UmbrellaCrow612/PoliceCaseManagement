using Cases.Core.Models;
using Cases.Core.ValueObjects;
using Pagination.Abstractions;
using Results.Abstractions;

namespace Cases.Core.Services
{
    /// <summary>
    /// Represents the business contract to interact with cases and there dependences
    /// </summary>
    public interface ICaseService
    {
        /// <summary>
        /// Create a case in the system.
        /// </summary>
        Task<CaseResult> CreateAsync(Case caseToCreate);

        /// <summary>
        /// Find a case by it's <see cref="Case.Id"/>
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        Task<Case?> FindByIdAsync(string caseId);

        /// <summary>
        /// Checks if a <see cref="Case.CaseNumber"/> is taken by another case
        /// </summary>
        /// <param name="caseNumber">The case number</param>
        /// <returns>Flag to indicate if a case number is taken</returns>
        Task<bool> IsCaseNumberTakenAsync(string caseNumber);

        /// <summary>
        /// Search for cases in the system with a query object
        /// </summary>
        /// <param name="query">The query object</param>
        /// <returns>Pagination result of cases</returns>
        Task<PaginatedResult<Case>> SearchCasesAsync(SearchCasesQuery query);

        /// <summary>
        /// Assigns the  user to a case
        /// </summary>
        /// <param name="case">The case to link to</param>
        /// <param name="userId">User id to link to it</param>
        Task<IResult> AddUserAsync(Case @case, string userId);

        /// <summary>
        /// Remove assigned user from a case
        /// </summary>
        /// <param name="userId">The ID of he user to remove</param>
        /// <param name="case">The case you want to remove them from</param>
        Task<CaseResult> RemoveUserAsync(Case @case, string userId);

        /// <summary>
        /// Get a list of <see cref="CaseAccessList"/> for the specific <see cref="Case"/> - these are all the people linked to them
        /// </summary>
        Task<List<CaseAccessList>> GetUsersAsync(Case @case);
    }

    /// <summary>
    /// Standard result object used in a case service function
    /// </summary>
    public class CaseResult : IResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<IResultError> Errors { get; set; } = [];

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new CaseError { Code = code, Message = message });
        }
    }

    /// <summary>
    /// Standard case service error shape
    /// </summary>
    public class CaseError : IResultError
    {
        public required string Code { get; set; }
        public string? Message { get; set; }
    }
}
