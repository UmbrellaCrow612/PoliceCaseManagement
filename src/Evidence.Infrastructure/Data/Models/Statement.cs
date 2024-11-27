namespace Evidence.Infrastructure.Data.Models
{
    public class Statement
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string StatementTakerName { get; set; }

        public required string WitnessName { get; set; }

        public DateTime StatementDateTime { get; set; }

        public required string Location { get; set; }

        public required string StatementText { get; set; }

        public required string Type { get; set; }

        public required string WitnessContactType { get; set; }
    }
}
