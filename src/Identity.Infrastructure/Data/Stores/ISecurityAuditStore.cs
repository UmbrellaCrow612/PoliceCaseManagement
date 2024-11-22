using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface ISecurityAuditStore
    {
        Task SetSecurityAudit(SecurityAudit audit);
        Task StoreSecurityAudit(SecurityAudit audit);
    }
}
