
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
