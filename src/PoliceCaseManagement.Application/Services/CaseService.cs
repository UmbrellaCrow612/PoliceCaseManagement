using AutoMapper;
using PoliceCaseManagement.Application.DTOs.Cases;
using PoliceCaseManagement.Application.Interfaces;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Application.Services
{
    public class CaseService(ICaseRepository<Case,string> caseRepository, IMapper mapper) : ICaseService
    {
        private readonly ICaseRepository<Case, string> _caseRepository = caseRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<CaseDto> CreateCaseAsync(string userId, CreateCaseDto newCase)
        {
            var caseToCreate = _mapper.Map<Case>(newCase);
            caseToCreate.CreatedById = userId;

            await _caseRepository.AddAsync(caseToCreate);

            return _mapper.Map<CaseDto>(caseToCreate);
        }

        public async Task<bool> DeleteCaseAsync(string caseId)
        {
            var isDeleted = await _caseRepository.DeleteAsync(caseId);

            return isDeleted;
        }

        public async Task<CaseDto?> GetCaseByIdAsync(string caseId)
        {
            var caseToGet = await _caseRepository.GetByIdAsync(caseId);

            return _mapper.Map<CaseDto>(caseToGet);
        }

        public async Task<bool> UpdateCaseAsync(string caseId, UpdateCaseDto updatedCase)
        {
            var caseToUpdate = await _caseRepository.GetByIdAsync(caseId);
            if (caseToUpdate is null) return false;

            _mapper.Map(updatedCase, caseToUpdate);

            await _caseRepository.UpdateAsync(caseToUpdate);

            return true;
        }
    }
}
