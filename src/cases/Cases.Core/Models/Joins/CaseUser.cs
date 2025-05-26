using Events;

namespace Cases.Core.Models.Joins
{
    /// <summary>
    /// Stores user info for linked users - stores a ref and de norm data in here
    /// </summary>
    public class CaseUser : IDenormalizedEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();


        [DenormalizedField("Application user", "Id", "Identity Service")]
        public required string UserId { get; set; }

        [DenormalizedField("Application user", "UserName", "Identity Service")]
        public required string UserName { get; set; }

        [DenormalizedField("Application user", "Email", "Identity Service")]
        public required string UserEmail { get; set; }


        public required string CaseId { get; set; }
        public Case Case { get; set; } = null!;
    }
}
