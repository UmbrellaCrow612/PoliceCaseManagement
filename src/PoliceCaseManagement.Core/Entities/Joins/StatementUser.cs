namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Join between a <see cref="Entities.Statement"/> and <see cref="Entities.User"/>
    /// </summary>
    public class StatementUser
    {
        public required string StatementId { get; set; }
        public required string UserId { get; set; }

        public User? User { get; set; } = null;
        public Statement? Statement { get; set; } = null;
    }
}
