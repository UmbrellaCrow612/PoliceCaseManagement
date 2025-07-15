using Cases.Core.Models;
using Cases.Core.Services;
using Cases.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Results.Abstractions;

namespace Cases.Application.Implementations
{
    internal class CaseActionService(CasesApplicationDbContext dbContext) : ICaseActionService
    {
        private readonly CasesApplicationDbContext _dbcontext = dbContext;

        public async Task<IResult> CreateAsync(Case @case, CaseAction action)
        {
            var result = new Result();
            await _dbcontext.CaseActions.AddAsync(action);
            await _dbcontext.SaveChangesAsync();
            return result;
        }

        public async Task<CaseAction?> FindByIdAsync(string caseActionId)
        {
            return await _dbcontext.CaseActions.FindAsync(caseActionId);
        }

        public async Task<List<CaseAction>> GetAsync(Case @case)
        {
            return await _dbcontext.CaseActions.Where(x => x.CaseId == @case.Id).ToListAsync();
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
