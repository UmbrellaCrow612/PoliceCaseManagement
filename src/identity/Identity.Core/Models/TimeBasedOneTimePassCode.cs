namespace Identity.Core.Models
{
    public class TimeBasedOneTimePassCode
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Secret { get; set; } // You encript it not hash its value currently it is base32 string
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;

        public ICollection<TimeBasedOneTimePassCodeBackupCode> TimeBasedOneTimePassCodeBackupCodes { get; set; } = [];
    }
}
