using Cases.Core.Models;
using Cases.Core.Services;
using Cases.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cases.Application.Implementations
{
    public class CaseAuthorizationService(CasesApplicationDbContext dbContext) : ICaseAuthorizationService
    {
        private readonly CasesApplicationDbContext _dbContext = dbContext;

        public async Task<bool> CanUserAddActions(string userId, string caseId)
        {
            return await _dbContext.CaseAccessLists.AnyAsync(
            x => x.CaseId == caseId
                 && x.UserId == userId
                 && (x.CaseRole == Core.Models.CaseRole.Editor
                     || x.CaseRole == Core.Models.CaseRole.Owner)
            );
        }

        public async Task<bool> CanUserAssignCaseUsers(string userId, string caseId)
        {
            return await _dbContext.CaseAccessLists.AnyAsync(x => x.UserId == userId && x.CaseId == caseId && x.CaseRole == CaseRole.Owner);
        }

        public async Task<bool> CanUserEditIncidentTypes(string userId, string caseId)
        {
            return await _dbContext.CaseAccessLists.AnyAsync(x => x.CaseId == caseId && x.UserId == userId 
                && (x.CaseRole == Core.Models.CaseRole.Editor
                     || x.CaseRole == Core.Models.CaseRole.Owner));
        }

        public async Task<bool> CanUserViewCase(string userId, string caseId)
        {
            return await _dbContext.CaseAccessLists.AnyAsync(x => x.UserId == userId && x.CaseId == caseId && (x.CaseRole == Core.Models.CaseRole.Editor
                   || x.CaseRole == Core.Models.CaseRole.Owner));
        }

        public async Task<bool> CanUserViewCaseActions(string userId, string caseId)
        {
            return await _dbContext.CaseAccessLists.AnyAsync(x => x.UserId == userId && x.CaseId == caseId && (x.CaseRole == Core.Models.CaseRole.Editor
                   || x.CaseRole == Core.Models.CaseRole.Owner));
        }

        public async Task<CaseRole> GetUserRole(Case @case, string userId)
        {
            var link = await _dbContext.CaseAccessLists.FirstOrDefaultAsync(x => x.CaseId == @case.Id && x.UserId == userId) ?? throw new InvalidOperationException("User not linked to case");
            return link.CaseRole;
        }
    }
}
