namespace Cases.Core.Models.Joins
{
    /// <summary>
    /// Join between <see cref="Case"/> and <see cref="IncidentType"/>
    /// A case can be linked to many types of <see cref="IncidentType"/> and a <see cref="IncidentType"/> can
    /// be part of many <see cref="Case"/>
    /// </summary>
    public class CaseIncidentType
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Case? Case { get; set; } = null;
        public required string CaseId { get; set; }


        public IncidentType IncidentType { get; set; } = null!;
        public required string IncidentTypeId { get; set; }
    }
}
