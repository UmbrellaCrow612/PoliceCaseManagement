using PoliceCaseManagement.Core.Entities.Interfaces;

namespace PoliceCaseManagement.Core.Entities
{
    public class Release : IAuditable
    {
        public required string PersonId { get; set; }
        public Person? Person { get; set; } = null;
        public required DateTime ReleaseDate { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastEditedAt { get; set; } = null;
        public required string CreatedById { get; set; }
        public string? LastEditedById { get; set; } = null;
        public User? CreatedBy { get; set; } = null;
        public User? LastEditedBy { get; set; } = null;
    }
}
