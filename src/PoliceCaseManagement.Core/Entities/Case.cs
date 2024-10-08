﻿using PoliceCaseManagement.Core.Entities.Enums;
using PoliceCaseManagement.Core.Entities.Join;
using PoliceCaseManagement.Core.Services;
using PoliceCaseManagement.Shared.Interfaces;

namespace PoliceCaseManagement.Core.Entities
{
    /// <summary>
    /// Entity <see cref="Case"/> in the system.
    /// </summary>
    public class Case : ISoftDeletable, IAuditable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CaseNumber { get; set; } = CaseNumberGeneratorService.GenerateCaseNumber();
        public CaseStatus CaseStatus { get; set; } = CaseStatus.Open;
        public required CasePriority CasePriority { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime? DateClosed { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastEditedAt { get; set; } = null;
        public required string CreatedById { get; set; }
        public string? LastEditedById { get; set; } = null;
        public DateTime? DeletedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public string? DeletedById { get; set; } = null;

        public ICollection<CasePerson> CasePersons { get; set; } = [];
        public ICollection<CaseUser> CaseUsers { get; set; } = [];
        public ICollection<CaseTag> CaseTags { get; set; } = [];
        public ICollection<Evidence> Evidences { get; set; } = [];
        public ICollection<CrimeScene> CrimeScenes { get; set; } = [];
        public ICollection<Report> Reports { get; set; } = [];
        public string? DepartmentId { get; set; } = null;
        public Department? Department { get; set; } = null;
        public required User CreatedBy { get; set; }
        public User? LastEditedBy { get; set; } = null;
    }
}
