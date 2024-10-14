namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Department
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public required string Description { get; set; }

        public ICollection<Case> AssignedCases { get; set; } = [];
        public ICollection<User> AssignedUsers { get; set; } = [];
    }
}
