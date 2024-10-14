namespace PoliceCaseManagement.Core.Entities.Interfaces
{
    /// <summary>
    /// Mark a entity as Auditable, must implement audit fields defined.
    /// </summary>
    public interface IAuditable
    {
        public DateTime CreatedAt { get; set; }

        public DateTime? LastEditedAt { get; set; }

        public string CreatedById { get; set; }

        public string? LastEditedById { get; set; }


        public User? CreatedBy { get; set; }

        public User? LastEditedBy { get; set; }
    }
}
