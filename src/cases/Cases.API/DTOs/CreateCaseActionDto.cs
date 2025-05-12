namespace Cases.API.DTOs
{
    public class CreateCaseActionDto
    {
        public required string Description { get; set; }
        public required string? Notes { get; set; } = null;
    }
}
