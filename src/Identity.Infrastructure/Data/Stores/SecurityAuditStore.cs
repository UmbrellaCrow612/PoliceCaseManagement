
using Identity.Core.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public class SecurityAuditStore(IdentityApplicationDbContext dbContext) : ISecurityAuditStore
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;

        public IQueryable<SecurityAudit> SecurityAudits => throw new NotImplementedException();

        public async Task SetSecurityAudit(SecurityAudit audit)
        {
            await _dbContext.SecurityAudits.AddAsync(audit);
        }

        public async Task StoreSecurityAudit(SecurityAudit audit)
        {
            await _dbContext.SecurityAudits.AddAsync(audit);
            await _dbContext.SaveChangesAsync();
        }
    }
}
