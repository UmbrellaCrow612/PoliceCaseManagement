using PoliceCaseManagement.Application.DTOs.Cases;

namespace PoliceCaseManagement.Application.Interfaces
{
    public interface ICaseService
    {
        /// <returns>Dto or <see langword="null"/> if it could not find it.</returns>
        Task<CaseDto?> GetCaseByIdAsync(string caseId);
        /// <returns>Dto of the created case.</returns>
        Task<CaseDto> CreateCaseAsync(string userId, CreateCaseDto newCase);
        /// <returns><see langword="true"/> if it could or <see langword="false"/> it it could not find it.</returns>
        Task<bool> UpdateCaseAsync(string caseId, UpdateCaseDto updatedCase);
        /// <returns><see langword="true"/> if it could or <see langword="false"/> it it could not find it.</returns>
        Task<bool> DeleteCaseAsync(string caseId);

    }
}
