using PoliceCaseManagement.Core.Entities.Join;

namespace PoliceCaseManagement.Core.Entities
{
    public class Tag
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public required string Description { get; set; }

        public ICollection<CaseTag> CaseTags { get; set; } = [];
    }
}
