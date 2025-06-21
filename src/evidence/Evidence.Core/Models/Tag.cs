using Evidence.Core.Models.Joins;

namespace Evidence.Core.Models
{
    /// <summary>
    /// Tag model used to tag or identify something
    /// </summary>
    public class Tag
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Nam of tag is unique 
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Optional description of the tag
        /// </summary>
        public required string? Description { get; set; } = null!;



        /// <summary>
        /// Many to Many link join table between <see cref="Models.Evidence"/> and <see cref="Models.Tag"/>
        /// </summary>
        public ICollection<EvidenceTag> EvidenceTags { get; set; } = [];
    }
}
