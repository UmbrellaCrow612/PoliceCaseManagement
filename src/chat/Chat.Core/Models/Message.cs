namespace Chat.Core.Models
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
