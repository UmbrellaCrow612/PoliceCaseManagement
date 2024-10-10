using PoliceCaseManagement.Core.Entities.Interfaces;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity <see cref="Booking"/> in the system.
    /// </summary>
    public class Booking : ISoftDeletable, IAuditable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public required string PersonId { get; set; }
        public Person? Person { get; set; } = null;
        public required string AssignedCell { get; set; }
        public required string Belongings { get; set; }
        public required string MugShotFileUrl { get; set; }
        public required string FingerprintFuleUrl { get; set; }
        public required string ArrestId { get; set; }
        public Arrest? Arrest { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastEditedAt { get; set; } = null;
        public required string CreatedById { get; set; }
        public string? LastEditedById { get; set; } = null;
        public User? CreatedBy { get; set; } = null;
        public User? LastEditedBy { get; set; } = null;

        public DateTime? DeletedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public string? DeletedById { get; set; } = null;
        public User? DeletedBy { get; set; } = null;
    }
}
