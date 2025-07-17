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
        Task<Case?> FindById(string caseId);

        /// <summary>
        /// Checks if a <see cref="Case.CaseNumber"/> is taken by another case
        /// </summary>
        /// <param name="caseNumber">The case number</param>
        /// <returns>Flag to indicate if a case number is taken</returns>
        Task<bool> IsCaseNumberTaken(string caseNumber);

        /// <summary>
        /// Search for cases in the system with a query object
        /// </summary>
        /// <param name="query">The query object</param>
        /// <returns>Pagination result of cases</returns>
        Task<PaginatedResult<Case>> SearchCases(SearchCasesQuery query);

        /// <summary>
        /// Assign user to a case
        /// </summary>
        /// <param name="case">The case to link to</param>
        /// <param name="userId">User id to link to it</param>
        Task<IResult> AddUser(Case @case, string userId);

        /// <summary>
        /// Remove assigned user from a case
        /// </summary>
        /// <param name="userId">The ID of he user to remove</param>
        /// <param name="case">The case you want to remove them from</param>
        Task<CaseResult> RemoveUser(Case @case, string userId);

        /// <summary>
        /// Get a list of <see cref="CaseAccessList"/> for the specific <see cref="Case"/> - these are all the people linked to them
        /// </summary>
        Task<List<CaseAccessList>> GetUsersAsync(Case @case);

        /// <summary>
        /// Check to see if a user is linked to a specific <see cref="Case"/>
        /// </summary>
        /// <param name="case">The case to check against</param>
        /// <param name="userId">The user to check</param>
        /// <returns>A bool to indicate if they are or are not</returns>
        Task<bool> IsUserLinkedToCase(Case @case, string userId);
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
