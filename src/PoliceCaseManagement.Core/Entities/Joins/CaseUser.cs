namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Joins entity between a <see cref="Case"/> and a <see cref="User"/> 
    /// </summary>
    public class CaseUser
    {
        public required string UserId { get; set; }
        public required string CaseId { get; set; }

        public User? User { get; set; } = null;
        public Case? Case { get; set; } = null;
    }
}
