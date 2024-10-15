using Microsoft.AspNetCore.Identity;
using PoliceCaseManagement.Core.Entities.Interfaces;
using PoliceCaseManagement.Core.Entities.Joins;

namespace PoliceCaseManagement.Core.Entities
{
    public class User : IdentityUser, ISoftDeletable
    {
        public required string Name { get; set; }
        public required string Rank { get; set; }
        public required string BadgeNumber { get; set; }
        public required string DepartmentId { get; set; }
        public Department? Department { get; set; } = null;

        public DateTime? DeletedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public string? DeletedById { get; set; } = null;
        public User? DeletedBy { get; set; } = null;

        public ICollection<Statement> CreatedStatements { get; set; } = [];
        public ICollection<Evidence> CreatedEvidence { get; set; } = [];
        public ICollection<Case> CreatedCases { get; set; } = [];
        public ICollection<Case> DeletedCases { get; set; } = [];
        public ICollection<CaseUser> CaseUsers { get; set; } = [];
        public ICollection<Report> CreatedReports { get; set; } = [];
        public ICollection<Report> DeletedReports { get; set; } = [];
        public ICollection<Report> LastEditedReports { get; set; } = [];
        public ICollection<Case> LastEditedCases { get; set; } = [];
    }
}