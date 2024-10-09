using PoliceCaseManagement.Core.Entities.Interfaces;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Report : ISoftDeletable, IAuditable
    {
        public DateTime? DeletedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public string? DeletedById { get; set; } = null;
        public User? DeletedBy { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastEditedAt { get; set; } = null;
        public required string CreatedById { get; set; };
        public string? LastEditedById { get; set; } = null;
        public User? CreatedBy { get; set; } = null;
        public User? LastEditedBy { get; set; } = null;
    }
}
