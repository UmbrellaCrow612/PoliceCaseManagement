namespace Chat.Core.Models
{
    public class Chat
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public required string RecipientId { get; set; }

        public ICollection<Message> Messages { get; set; } = [];
    }
}
