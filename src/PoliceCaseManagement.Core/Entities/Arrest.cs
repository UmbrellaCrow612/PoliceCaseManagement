using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity <see cref="Arrest"/> in the system.
    /// </summary>
    public class Arrest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string ReasonOfArrest { get; set; }
        public required DateTime ArrestDateTime { get; set; }
        /// <summary>
        /// Where they were arrested.
        /// </summary>
        public required string LocationId { get; set; }
        public required Location Location { get; set; }
        /// <summary>
        /// Person being arrested.
        /// </summary>
        public required string PersonId { get; set; }
        public Person? Person { get; set; } = null;


        public ICollection<ArrestUser> ArrestUsers = [];
    }
}
