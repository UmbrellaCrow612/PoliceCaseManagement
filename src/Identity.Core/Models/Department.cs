namespace Identity.Core.Models
{
    public class Department
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public ICollection<ApplicationUser> Users { get; set; } = [];
    }
}
