namespace Evidence.Infrastructure.Data.Models
{
    public interface ISoftDelete
    {
        public string? DeletedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
