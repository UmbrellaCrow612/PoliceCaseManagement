namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity <see cref="Charge"/> in the system.
    /// </summary>
    public class Charge
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Description { get; set; }
        public required string LegalCode { get; set; }
        public DateTime ChargeDate = DateTime.UtcNow;
        public required string ArrestId { get; set; }
        public Arrest? Arrest { get; set; } = null;
    }
}
