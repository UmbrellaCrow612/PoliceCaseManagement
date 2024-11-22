namespace Identity.Infrastructure.Data.Models
{
    /// <summary>
    /// Record security-related events Track permission changes Log administrative actions Capture role modifications
    /// </summary>
    public class SecurityAudit
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required SecurityAuditEvent Event { get; set; }
        public string? UserId { get; set; } = null;
        public ApplicationUser? User { get; set; } = null;
        public string? Details { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public SecurityAuditSeverity Severity { get; set; } = SecurityAuditSeverity.Low;
        public string? PreviousValue { get; set; } = null;
        public string? NewValue { get; set; } = null;
        public string? AffectedResource { get; set; } = null;
        public string? IPAddress { get; set; } = null;
        public string? Changes { get; set; } = null;
    }

    public enum SecurityAuditEvent
    {
        None = 0,
        Authorization = 1
    }

    public enum SecurityAuditSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }
}
