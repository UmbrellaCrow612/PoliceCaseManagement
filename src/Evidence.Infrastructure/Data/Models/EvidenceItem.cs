namespace Evidence.Infrastructure.Data.Models
{
    public class EvidenceItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
