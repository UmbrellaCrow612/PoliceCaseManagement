using Cases.Core.Models;
using Cases.Core.Services;
using Cases.Infrastructure.Data;

namespace Cases.Application.Implementations
{
    internal class CaseActionService(CasesApplicationDbContext dbContext) : ICaseActionService
    {
        private readonly CasesApplicationDbContext _dbcontext = dbContext;

        public async Task<CaseAction?> GetCaseActionByIdAsync(string caseActionId)
        {
            return await _dbcontext.CaseActions.FindAsync(caseActionId);
        }
    }
}
