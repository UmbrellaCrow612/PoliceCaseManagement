
using Cases.Core.Models;

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
