using Events.Core;

namespace Cases.Core.Models
{
    /// <summary>
    /// A list for a specific <see cref="Core.Models.Case"/> contains all the users and there role on the given case - also used as a way of knowing if they 
    /// are assigned to it
    /// </summary>
    public class CaseAccessList : IDenormalizedEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string CaseId { get; set; }
        public Case? Case { get; set; } = null;


        [DenormalizedField("Application user", "Id", "Identity Service")]
        public required string UserId { get; set; }

        [DenormalizedField("Application user", "UserName", "Identity Service")]
        public required string UserName { get; set; }

        [DenormalizedField("Application user", "Email", "Identity Service")]
        public required string UserEmail { get; set; }


        public required CaseRole CaseRole { get; set; }
    }
}
