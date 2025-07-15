using Cases.Core.Models;

namespace Cases.API.DTOs
{
    public class AssignUserToCaseDto
    {
        public required string UserId { get; set; }
        public required CaseRole CaseRole { get; set; }
    }
}
