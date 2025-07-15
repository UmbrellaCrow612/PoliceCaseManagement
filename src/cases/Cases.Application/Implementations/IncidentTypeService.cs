using Cases.Application.Codes;
using Cases.Core.Models;
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

        public Task<IResult> DeleteAsync(IncidentType incidentType)
        {
            throw new NotImplementedException();
        }

        public Task<IncidentType?> FindByIdAsync(string incidentTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<List<IncidentType>> GetAsync(Case @case)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> LinkToCase(Case @case, IncidentType incidentType)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<IncidentType>> SearchAsync(SearchIncidentTypesQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAsync(IncidentType incidentType)
        {
            throw new NotImplementedException();
        }


        private class Result : IResult
        {
            public bool Succeeded { get; set; } = false;
            public ICollection<IResultError> Errors { get; set; } = [];

            public void AddError(string code, string? message = null)
            {
                Errors.Add(new Error { Code = code, Message = message });
            }
        }

        private class Error : IResultError
        {
            public required string Code { get; set; }
            public required string? Message { get; set; } = null;
        }
    }
}
