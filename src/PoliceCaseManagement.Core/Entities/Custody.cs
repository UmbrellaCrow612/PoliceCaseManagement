namespace PoliceCaseManagement.Core.Entities
{
    public class Custody
    {
        public required string PersonId { get; set; }
        public Person? Person { get; set; } = null;
    }
}
