namespace Challenge.Core.Models
{
    /// <summary>
    /// Represents a challenge claim, for example a claim that is used to generate a challenge token.
    /// </summary>
    public class ChallengeClaim
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
    }
}
