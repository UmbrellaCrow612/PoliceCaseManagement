using Cases.Application.Codes;
using Cases.Core.Models;
using Cases.Core.Models.Joins;
using Cases.Core.Services;
using Cases.Core.ValueObjects;
using Cases.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Pagination.Abstractions;
using Results.Abstractions;

namespace Cases.Application.Implementations
{
    public class IncidentTypeService(CasesApplicationDbContext dbContext) : IIncidentTypeService
    {
        private readonly CasesApplicationDbContext _dbContext = dbContext;
        
        public async Task<int> CountCaseLinks(IncidentType incidentType)
        {
            return await _dbContext.CaseIncidentTypes.Where(x => x.IncidentTypeId == incidentType.Id).CountAsync();
        }

        public async Task<IResult> CreateAsync(IncidentType incidentType)
        {
            var result = new Result();

            var isNameTaken = await _dbContext.IncidentTypes.AnyAsync(x => x.Name == incidentType.Name.ToLower());
            if (isNameTaken)
            {
                result.AddError(BusinessRuleCodes.IncidentTypeAlreadyExists, "Incident type already exists");
                return result;
            }

            await _dbContext.IncidentTypes.AddAsync(incidentType);
            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<IResult> DeleteAsync(IncidentType incidentType)
        {
            var result = new Result();

            await using var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var joins = await _dbContext.CaseIncidentTypes.Where(x => x.IncidentTypeId == incidentType.Id).ExecuteDeleteAsync();

                _dbContext.IncidentTypes.Remove(incidentType);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                result.AddError(BusinessRuleCodes.ValidationError, "DB fail");
                return result;
            }
            
            result.Succeeded = true;
            return result;
        }

        public async Task<IncidentType?> FindByIdAsync(string incidentTypeId)
        {
            return await _dbContext.IncidentTypes.FindAsync(incidentTypeId);
        }

        public async Task<List<IncidentType>> GetAsync(Case @case)
        {
            return await _dbContext.CaseIncidentTypes.Where(x => x.CaseId == @case.Id).Select(x => x.IncidentType).ToListAsync();
        }

        public async Task<IResult> LinkToCase(Case @case, IncidentType incidentType)
        {
            var result = new Result();

            var alreadyLinkedToCase = await _dbContext.CaseIncidentTypes.AnyAsync(x => x.CaseId == @case.Id && x.IncidentTypeId == incidentType.Id);
            if (alreadyLinkedToCase)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Incident type already linked to case");
                return result;
            }

            var link = new CaseIncidentType
            {
                CaseId = @case.Id,
                IncidentTypeId = incidentType.Id,
            };
            await _dbContext.CaseIncidentTypes.AddAsync(link);
            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public Task<PaginatedResult<IncidentType>> SearchAsync(SearchIncidentTypesQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> UpdateAsync(IncidentType incidentType)
        {
            var result = new Result();

            _dbContext.IncidentTypes.Update(incidentType);
            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }
    }
}
