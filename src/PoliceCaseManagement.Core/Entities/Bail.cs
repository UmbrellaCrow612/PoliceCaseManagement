using PoliceCaseManagement.Core.Entities.Interfaces;

namespace PoliceCaseManagement.Core.Entities
{
    public class Bail : ISoftDeletable, IAuditable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public decimal Amount { get; set; }
        public required string Conditions { get; set; }
        public required bool PostedStatus { get; set; }
        public required DateTime CourtDate { get; set; }
        public required string ArrestId { get; set; }
        public Arrest? Arrest { get; set; } = null;

        public DateTime? DeletedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public string? DeletedById { get; set; } = null;
        public User? DeletedBy { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastEditedAt { get; set; } = null;
        public required string CreatedById { get; set; }
        public string? LastEditedById { get; set; } = null;
        public User? CreatedBy { get; set; } = null;
        public User? LastEditedBy { get; set; } = null;
    }
}
