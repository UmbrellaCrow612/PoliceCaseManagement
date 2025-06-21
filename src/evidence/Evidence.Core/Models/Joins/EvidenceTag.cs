namespace Evidence.Core.Models.Joins
{
    /// <summary>
    /// Join table between the many to many link between a <see cref="Models.Evidence"/> and <see cref="Tag"/>
    /// </summary>
    public class EvidenceTag
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The specific <see cref="Models.Evidence"/> this is for
        /// </summary>
        public required string EvidenceId { get; set; }
        public Models.Evidence Evidence { get; set; } = null!;


        /// <summary>
        /// The specific <see cref="Models.Tag"/> this is for
        /// </summary>
        public required string TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
