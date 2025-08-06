using Cases.Core.ValueObjects;
using Events.Core;

namespace Cases.Core.Models.Joins
{
    /// <summary>
    /// Join table between a case and a person - holds there de norm data and role on that case
    /// </summary>
    public class CasePerson : IDenormalizedEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string CaseId { get; set; }
        public Case Case { get; set; } = null!;
        public required CasePersonRole Role { get; set; }


        // De norm local copy  of person details

        [DenormalizedField("People Service", "Id", "People Service")]
        public required string PersonId { get; set; }

        [DenormalizedField("People Service", "firstName", "People Service")]
        public required string PersonFirstName { get; set; }

        [DenormalizedField("People Service", "lastName", "People Service")]
        public required string PersonLastName { get; set; }

        [DenormalizedField("People Service", "dateOfBirth", "People Service")]
        public required DateTime PersonDateBirth { get; set; }

        [DenormalizedField("People Service", "phoneNumber", "People Service")]
        public required string PersonPhoneNumber { get; set; }

        [DenormalizedField("People Service", "email", "People Service")]
        public required string PersonEmail { get; set; }
    }
}
