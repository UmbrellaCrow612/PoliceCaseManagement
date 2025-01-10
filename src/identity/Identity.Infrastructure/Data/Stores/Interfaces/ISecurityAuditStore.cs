using Identity.Core.Models;

namespace Identity.Infrastructure.Data.Stores.Interfaces
{
    public interface ISecurityAuditStore
    {
        IQueryable<SecurityAudit> SecurityAudits { get; }

        Task SetSecurityAudit(SecurityAudit audit);
        Task StoreSecurityAudit(SecurityAudit audit);
    }
}
