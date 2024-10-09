namespace PoliceCaseManagement.Core.Entities
{
    public class Release
    {
        public required string PersonId { get; set; }
        public Person? Person { get; set; } = null;
    }
}
