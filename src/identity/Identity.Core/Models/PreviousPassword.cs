namespace Identity.Core.Models
{
    /// <summary>
    /// Model to store a <see cref="ApplicationUser"/> previous password - used on change password 
    /// to match agaisnt previous passwords used by a user when they want to change it 
    /// or are forced to when they current password expires
    /// </summary>
    public class PreviousPassword
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required string PasswordHash { get; set; }

        public ApplicationUser? User { get; set; } = null;
        public required string UserId { get; set; }
    }
}
