namespace Evidence.Infrastructure.Data.Models
{
    public interface IAudit
    {
        public string CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LastEditedById { get; set; }
        public DateTime? LastEditedAt { get; set; }
    }
}
