using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public class SecurityAuditStore(IdentityApplicationDbContext dbContext) : ISecurityAuditStore
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;

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
