using Cases.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Cases.API.DTOs
{
    public class AssignPersonToCase
    {
        [Required]
        public required string PersonId { get; set; }

        [Required]
        public required CasePersonRole Role { get; set; }
    }
}
