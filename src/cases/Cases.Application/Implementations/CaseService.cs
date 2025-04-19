using Cases.Application.Codes;
using Cases.Core.Models;
using Cases.Core.Models.Joins;
using Cases.Core.Services;
using Cases.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cases.Application.Implementations
{
    internal class CaseService(CasesApplicationDbContext dbContext) : ICaseService
    {
        private readonly CasesApplicationDbContext _dbcontext = dbContext;

        public async Task<CaseResult> AddToIncidentType(Case @case, IncidentType incidentType)
        {
            var result = new CaseResult();

            var isIncidentTypeAlreadyLinkedToCase = await _dbcontext.CaseIncidentTypes.AnyAsync(x => x.CaseId == @case.Id && x.IncidentTypeId == incidentType.Id);
            if (isIncidentTypeAlreadyLinkedToCase)
            {
                result.AddError(BusinessRuleCodes.IncidentTypeAlreadyLinkedToCase, $@"Incident type ${incidentType.Name} already linked to case.");
                return result;
            }

            var join = new CaseIncidentType
            {
                CaseId = @case.Id,
                IncidentTypeId = incidentType.Id
            };
            await _dbcontext.CaseIncidentTypes.AddAsync(join);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> CreateAsync(Case caseToCreate)
        {
            var result = new CaseResult();

            if (!string.IsNullOrWhiteSpace(caseToCreate.CaseNumber))
            {
                var isCaseNumberTaken = await _dbcontext.Cases.AnyAsync(x => x.CaseNumber == caseToCreate.CaseNumber);
                if (isCaseNumberTaken)
                {
                    result.AddError(BusinessRuleCodes.CaseNumberTaken, $"Case number: ${caseToCreate.CaseNumber} is taken.");
                    return result;
                }
            }

            await _dbcontext.Cases.AddAsync(caseToCreate);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> CreateIncidentType(IncidentType incidentType)
        {
            var result = new CaseResult();

            var incidentTypeAlreadyExists = await _dbcontext.IncidentTypes.AnyAsync(x => x.Name == incidentType.Name);
            if (incidentTypeAlreadyExists)
            {
                result.AddError(BusinessRuleCodes.IncidentTypeAlreadyExists, "Trying to create a Incident Type that already exists.");
                return result;
            }

            await _dbcontext.IncidentTypes.AddAsync(incidentType);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> DeleteIncidentType(IncidentType incidentType)
        {
            var result = new CaseResult();

            var joins = await _dbcontext.CaseIncidentTypes.Where(x => x.IncidentTypeId == incidentType.Id).ToListAsync();
            _dbcontext.RemoveRange(joins);
            _dbcontext.Remove(incidentType);

            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<Case?> FindById(string caseId)
        {
            return await _dbcontext.Cases.FindAsync(caseId);
        }

        public async Task<IncidentType?> FindIncidentTypeById(string incidentTypeId)
        {
            return await _dbcontext.IncidentTypes.FindAsync(incidentTypeId);
        }

        public async Task<List<IncidentType>> GetAllIncidentTypes()
        {
            return await _dbcontext.IncidentTypes.ToListAsync();
        }

        public async Task<int> GetCaseIncidentCount(IncidentType incidentType)
        {
            return await _dbcontext.CaseIncidentTypes.Where(x => x.IncidentTypeId == incidentType.Id).CountAsync();
        }

        public async Task<CaseResult> UpdateIncidentType(IncidentType incidentType)
        {
            var result = new CaseResult();

            var isIncidentTypeNameTaken = await _dbcontext.IncidentTypes.AnyAsync(x => x.Name == incidentType.Name && x.Id != incidentType.Id);
            if (isIncidentTypeNameTaken)
            {
                result.AddError(BusinessRuleCodes.IncidentTypeAlreadyExists, $@"Incident type {incidentType.Name} is already taken, try another.");
                return result;
            }

            _dbcontext.IncidentTypes.Update(incidentType);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded= true;
            return result;
        }
    }
}
