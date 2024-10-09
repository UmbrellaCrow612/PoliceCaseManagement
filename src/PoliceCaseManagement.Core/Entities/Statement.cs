namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Statement
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string PersonId { get; set; }
        public Person? person { get; set; } = null;
    }
}
