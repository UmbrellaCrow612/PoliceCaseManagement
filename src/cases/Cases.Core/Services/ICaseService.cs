using Cases.Core.Models;
using Cases.Core.Models.Joins;
using Cases.Core.ValueObjects;

namespace Cases.Core.Services
{
    public interface ICaseService
    {
        /// <summary>
        /// Create a case in the system.
        /// </summary>
        Task<CaseResult> CreateAsync(Case caseToCreate);

        /// <summary>
        /// Create a IncidentType that cases can link to.
        /// </summary>
        Task<CaseResult> CreateIncidentType(IncidentType incidentType);

        /// <summary>
        /// Link a <see cref="Case"/> to a <see cref="IncidentType"/> through the join table <see cref="Cases.Core.Models.Joins.CaseIncidentType"/>
        /// </summary>
        /// <returns></returns>
        Task<CaseResult> AddToIncidentType(Case @case, IncidentType incidentType);

        /// <summary>
        /// Adds a <see cref="CaseAction"/> to a <see cref="Case"/> case actions have a one to many relation to cases only one action belongs to one case
        /// </summary>
        /// <param name="case">The case you want to add a case action to</param>
        /// <param name="caseAction">The action you want to record</param>
        Task<CaseResult> AddCaseAction(Case @case, CaseAction caseAction);

        /// <summary>
        /// Find a case by it's <see cref="Case.Id"/>
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        Task<Case?> FindById(string caseId);

        /// <summary>
        /// Find a incident type by it's ID.
        /// </summary>
        Task<IncidentType?> FindIncidentTypeById(string incidentTypeId);

        /// <summary>
        /// Get all <see cref="IncidentType"/> defined in the system that a <see cref="Case"/> can link to.
        /// </summary>
        /// <returns></returns>
        Task<List<IncidentType>> GetAllIncidentTypes();

        /// <summary>
        /// Get the number of times a <see cref="IncidentType"/> is linked to x many cases through the join table <see cref="Models.Joins.CaseIncidentType"/>
        /// </summary>
        /// <param name="incidentType">The incident type you want to know how many cases it is linked to.</param>
        /// <returns></returns>
        Task<int> GetCaseIncidentCount(IncidentType incidentType);

        /// <summary>
        /// Delete a incident type - unlinks it from any cases it's linked to and then deleted.
        /// </summary>
        Task<CaseResult> DeleteIncidentType(IncidentType incidentType);

        /// <summary>
        /// Update a incident type.
        /// </summary>
        Task<CaseResult> UpdateIncidentType(IncidentType incidentType);

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
        Task<PagedResult<Case>> SearchCases(SearchCasesQuery query);

        /// <summary>
        /// Get <see cref="IncidentType"/> linked to the <see cref="Case"/> passed in through the join table <see cref="Models.Joins.CaseIncidentType"/>
        /// </summary>
        /// <param name="case">The case you want to the get the linked incident types to it</param>
        /// <returns>List of incident types</returns>
        Task<List<IncidentType>> GetIncidentTypes(Case @case);

        /// <summary>
        /// Update a cases linked incident types through the join table case incident types
        /// it removes all currently linked incident types and then links the passed incident types to it
        /// </summary>
        /// <param name="case">The case you want to update</param>
        /// <param name="incidentTypes">List of new incident types you want to link to it</param>
        Task<CaseResult> UpdateCaseLinkedIncidentTypes(Case @case, List<IncidentType> incidentTypes);

        /// <summary>
        /// Get all case actions for a given case
        /// </summary>
        /// <param name="case">The case you want to get all case actions for</param>
        Task<List<CaseAction>> GetCaseActions(Case @case);

        /// <summary>
        /// Get all users linked to a case
        /// </summary>
        /// <param name="case">Case you want to get info for</param>
        Task<List<CaseUser>> GetCaseUsers(Case @case);

        /// <summary>
        /// Assign a set of users to a case
        /// </summary>
        /// <param name="case">The case to link to</param>
        /// <param name="userIds">A set of user id's to link to it</param>
        Task<CaseResult> AddUsers(Case @case, List<string> userIds);
    }

    /// <summary>
    /// Standard result object used in a case service function
    /// </summary>
    public class CaseResult : IServiceResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<IServiceError> Errors { get; set; } = [];

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new CaseError { Code = code, Message = message });
        }
    }

    /// <summary>
    /// Standard case service error shape
    /// </summary>
    public class CaseError : IServiceError
    {
        public required string Code { get; set; }
        public string? Message { get; set; }
    }
}
