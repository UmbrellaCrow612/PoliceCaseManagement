using PoliceCaseManagement.Application.Interfaces;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Application.Services
{
    public class CaseService(ICaseRepository<Case,string> caseRepository) : ICaseService
    {
        private readonly ICaseRepository<Case, string> _caseRepository = caseRepository;

        public Task<Case> CreateCaseAsync(Case newCase)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCaseAsync(string caseId)
        {
            throw new NotImplementedException();
        }

        public Task<Case> GetCaseByIdAsync(string caseId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCaseAsync(Case updatedCase)
        {
            throw new NotImplementedException();
        }
    }
}
