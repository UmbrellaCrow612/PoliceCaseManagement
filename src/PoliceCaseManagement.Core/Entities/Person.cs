using PoliceCaseManagement.Core.Entities.Interfaces;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public class Person : ISoftDeletable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string FullName { get; set; }
        public required string ContactInfo { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required string Address { get; set; }

        public DateTime? DeletedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public string? DeletedById { get; set; } = null;
        public User? DeletedBy { get; set; } = null;

        public ICollection<Arrest> Arrests { get; set; } = [];
        public ICollection<Booking> Bookings { get; set; } = [];
        public ICollection<Statement> Statements { get; set; } = [];
        public ICollection<CrimeScenePerson> CrimeScenePersons { get; set; } = [];
        public ICollection<CasePerson> CasePersons { get; set; } = [];
    }
}
