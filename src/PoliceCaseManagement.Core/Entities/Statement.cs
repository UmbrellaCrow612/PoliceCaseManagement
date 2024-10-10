using PoliceCaseManagement.Core.Entities.Interfaces;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Statement : IAuditable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Person giving the statement 
        /// </summary>
        public required string PersonId { get; set; }
        public Person? Person { get; set; } = null;
        /// <summary>
        /// List of Interviewer's
        /// </summary>
        public ICollection<StatementUser> StatementUsers { get; set; } = [];
        public required string Summary { get; set; }
        public required string RecordingFileUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastEditedAt { get; set; } = null;
        public required string CreatedById { get; set; }
        public string? LastEditedById { get; set; } = null;
        public User? CreatedBy { get; set; } = null;
        public User? LastEditedBy { get; set; } = null;
    }
}
