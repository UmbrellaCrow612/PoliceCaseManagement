using Cases.Core.Models;

namespace Cases.API.Mappings
{
    public class CaseUserDto
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public required string CaseId { get; set; }
    }
}
