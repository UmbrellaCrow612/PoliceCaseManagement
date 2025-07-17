using Cases.Core.Models;

namespace Cases.API.DTOs
{
    public class CaseAccessListDto
    {
        public required string Id { get; set; }

        public required string CaseId { get; set; }

        public required string UserId { get; set; }

        public required string UserName { get; set; }

        public required string UserEmail { get; set; }

        public required CaseRole CaseRole { get; set; }
    }
}
