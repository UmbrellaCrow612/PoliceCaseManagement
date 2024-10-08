namespace PoliceCaseManagement.Shared.Interfaces
{
    /// <summary>
    /// Mark a entity as being only soft deletable.
    /// </summary>
    public interface ISoftDeletable
    {
        public DateTime? DeletedAt { get; set; }

        public bool IsDeleted { get; set; }

        public string? DeletedById { get; set; }
    }
}
