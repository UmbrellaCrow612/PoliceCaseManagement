using PoliceCaseManagement.Application.Interfaces;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Exceptions;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Application.Services
{
    public class CaseService(ICaseRepository<Case,string> caseRepository) : ICaseService
    {
        private readonly ICaseRepository<Case, string> _caseRepository = caseRepository;

        public async Task CreateCaseAsync(Case newCase)
        {
            await _caseRepository.AddAsync(newCase);
        }

        public async Task DeleteCaseAsync(string caseId)
        {
            var caseExists = await _caseRepository.ExistsAsync(caseId);
            if (!caseExists) throw new BusinessRuleException("Case dose not exist");

            var caseToDelete = await _caseRepository.GetCaseWithDetailsByIdAsync(caseId) ?? throw new BusinessRuleException("Case dose not exist");

            if (caseToDelete.CaseUsers.Count > 0) throw new BusinessRuleException("Users linked to this case.");

            await _caseRepository.DeleteAsync(caseId);

        }

        public async Task<Case?> GetCaseByIdAsync(string caseId)
        {
            var caseToGet = await _caseRepository.GetByIdAsync(caseId);
            return caseToGet;
        }

        public async Task UpdateCaseAsync(Case updatedCase)
        {
            await _caseRepository.UpdateAsync(updatedCase);
        }
    }
}
