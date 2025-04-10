namespace Cases.API.DTOs
{
    public class IncidentTypeDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; } = null;
    }
}
