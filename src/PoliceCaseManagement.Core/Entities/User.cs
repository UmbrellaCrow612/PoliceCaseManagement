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
        public string? DepartmentId { get; set; } = null;
        public Department? Department { get; set; } = null;

        public DateTime? DeletedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public string? DeletedById { get; set; } = null;
        public User? DeletedBy { get; set; } = null;

        public ICollection<StatementUser> StatementUsers { get; set; } = [];
        public ICollection<Statement> CreatedStatements { get; set; } = [];
        public ICollection<Evidence> CreatedEvidences { get; set; } = [];
        public ICollection<Case> CreatedCases { get; set; } = [];
        public ICollection<Case> DeletedCases { get; set; } = [];
        public ICollection<CaseUser> CaseUsers { get; set; } = [];
        public ICollection<Report> CreatedReports { get; set; } = [];
        public ICollection<Report> DeletedReports { get; set; } = [];
        public ICollection<Report> LastEditedReports { get; set; } = [];
        public ICollection<Case> LastEditedCases { get; set; } = [];
        public ICollection<CrimeScene> DeletedCrimeScenes { get; set; } = [];
        public ICollection<Document> CreatedDocuments { get; set; } = [];
        public ICollection<Document> LastEditedDocuments { get; set; } = [];
        public ICollection<Document> DeletedDocuments { get; set; } = [];
        public ICollection<Evidence> LastEditedEvidences { get; set; } = [];
        public ICollection<Evidence> DeletedEvidences { get; set; } = [];
        public ICollection<Person> DeletedPersons { get; set; } = [];
        public ICollection<User> DeletedUsers { get; set; } = [];
        public ICollection<Statement> LastEditedStatements { get; set; } = [];
    }
}