namespace Cases.Core.Models.Joins
{
    /// <summary>
    /// Stores user info for linked users - stores a ref and de norm data in here
    /// </summary>
    public class CaseUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public required string CaseId { get; set; }
        public Case Case { get; set; } = null!;
    }
}
