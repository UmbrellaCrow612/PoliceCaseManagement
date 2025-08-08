using Cases.Core.ValueObjects;

namespace Cases.API.DTOs
{
    public class CasePersonDto
    {
        public required string Id { get; set; }
        public required string CaseId { get; set; }
        public required CasePersonRole Role { get; set; }
        public required string PersonId { get; set; }
        public required string PersonFirstName { get; set; }
        public required string PersonLastName { get; set; }
        public required DateTime PersonDateBirth { get; set; }
        public required string PersonPhoneNumber { get; set; }
        public required string PersonEmail { get; set; }
    }
}
