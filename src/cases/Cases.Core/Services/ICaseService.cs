using Cases.Core.Models;
using Cases.Core.Models.Joins;
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
        Task<IResult> CreateAsync(Case caseToCreate);

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
        Task<IResult> RemoveUserAsync(Case @case, string userId);

        /// <summary>
        /// Get a list of <see cref="CaseAccessList"/> for the specific <see cref="Case"/> - these are all the people linked to them
        /// </summary>
        Task<List<CaseAccessList>> GetUsersAsync(Case @case);

        /// <summary>
        /// Get all evidence linked to a case <see cref="CaseEvidence"/>
        /// </summary>
        /// <param name="case">The case to get the linked evidence for</param>
        /// <returns>List of evidence</returns>
        Task<List<CaseEvidence>> GetEvidenceAsync(Case @case);

        /// <summary>
        /// Link a piece of evidence to a given <see cref="Case"/>
        /// </summary>
        /// <param name="case">The case to link it to</param>
        /// <param name="evidenceId">The ID of the evidence to link</param>
        /// <returns>Result object</returns>
        Task<IResult> AddEvidenceAsync(Case @case, string evidenceId);

        /// <summary>
        /// Remove a piece of linked evidence from a case
        /// </summary>
        /// <param name="case">The case to remove the evidence from</param>
        /// <param name="evidenceId">The ID of the evidence to remove</param>
        /// <returns>Result object</returns>
        Task<IResult> RemoveEvidenceAsync(Case @case, string evidenceId);

        /// <summary>
        /// Links a person to a case and gives them a role on that case
        /// </summary>
        /// <param name="case">Teh case to link it to</param>
        /// <param name="personId">The ID of the person to link</param>
        /// <param name="role">The role they have on the case</param>
        /// <returns>Result object</returns>
        Task<IResult> AddPerson(Case @case, string personId, CasePersonRole role);

        /// <summary>
        /// Get a list of case people / persons linked to a given case
        /// </summary>
        /// <param name="case">The case to fetch for</param>
        /// <returns>List of case person's / people linked ot the case</returns>
        Task<List<CasePerson>> GetPeopleAsync(Case @case);
    }
}
