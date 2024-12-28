using Identity.Core.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface ISecurityAuditStore
    {
        IQueryable<SecurityAudit> SecurityAudits { get; }

        Task SetSecurityAudit(SecurityAudit audit);
        Task StoreSecurityAudit(SecurityAudit audit);
    }
}
