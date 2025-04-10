using Cases.Application.Codes;
using Cases.Core.Models;
using Cases.Core.Services;
using Cases.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cases.Application.Implementations
{
    internal class CaseService(CasesApplicationDbContext dbContext) : ICaseService
    {
        private readonly CasesApplicationDbContext _dbcontext = dbContext;

        public async Task<CaseResult> CreateAsync(Case caseToCreate)
        {
            var result = new CaseResult();

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
    }
}
