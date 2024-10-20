using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Application.Interfaces
{
    public interface ICaseService
    {
        Task<Case> GetCaseByIdAsync(string caseId);
        Task<Case> CreateCaseAsync(Case newCase);
        Task UpdateCaseAsync(Case updatedCase);
        Task DeleteCaseAsync(string caseId);

    }
}
