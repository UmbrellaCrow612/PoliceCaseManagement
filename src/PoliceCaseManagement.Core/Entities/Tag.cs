using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    public class Tag
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<CaseTag> CaseTags { get; set; } = [];
    }
}
