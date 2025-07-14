using Cases.Core.Models;
using Results.Abstractions;
using Pagination.Abstractions;
using Cases.Core.ValueObjects;

namespace Cases.Core.Services
{
    /// <summary>
    /// Handles all <see cref="IncidentType"/> business logic
    /// </summary>
    public interface IIncidentTypeService
    {
        Task<IResult> CreateAsync(IncidentType incidentType);

        Task<PaginatedResult<IncidentType>> SearchAsync(SearchIncidentTypesQuery query);

        Task<IncidentType?> FindByIdAsync(string incidentTypeId);

        Task<int> CountCaseLinks(IncidentType incidentType);

        Task<IResult> DeleteAsync(IncidentType incidentType);

        Task<IResult> UpdateAsync(IncidentType incidentType);

        Task<List<IncidentType>> GetAsync(Case @case);

        Task<IResult> LinkToCase(Case @case, IncidentType incidentType);
    }
}
