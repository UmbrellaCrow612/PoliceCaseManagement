using Cases.Core.Models;
using Cases.Core.Services;
using Cases.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cases.Application.Implementations
{
    public class CaseAuthorizationService(CasesApplicationDbContext dbContext) : ICaseAuthorizationService
    {
        private readonly CasesApplicationDbContext _dbContext = dbContext;

        public async Task<CaseRole> GetUserRole(Case @case, string userId)
        {
            var link = await _dbContext.CaseAccessLists.FirstOrDefaultAsync(x => x.CaseId == @case.Id && x.UserId == userId) ?? throw new InvalidOperationException("User not linked to case");
            return link.CaseRole;
        }

        public async Task<bool> IsAssigned(string userId, string caseId)
        {
            return await _dbContext.CaseAccessLists.AnyAsync(x => x.UserId == userId && x.CaseId == caseId);
        }

        public async Task<bool> IsEditor(string userId, string caseId)
        {
            return await _dbContext.CaseAccessLists.AnyAsync(x => x.UserId == userId && x.CaseId == caseId && (x.CaseRole == CaseRole.Editor || x.CaseRole == CaseRole.Owner));
        }

        public async Task<bool> IsOwner(string userId, string caseId)
        {
            return await _dbContext.CaseAccessLists.AnyAsync(x => x.UserId == userId && x.CaseId == caseId && x.CaseRole == CaseRole.Owner);
        }
    }
}
