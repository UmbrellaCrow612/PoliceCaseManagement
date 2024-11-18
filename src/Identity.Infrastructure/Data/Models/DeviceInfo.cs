namespace Identity.Infrastructure.Data.Models
{
    public class DeviceInfo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserAgent { get; set; }
        public required string IpAddress { get; set; }
        public string? DeviceId { get; set; } = null;
        public string? DeviceType { get; set; } = null;
        public string? Browser { get; set; } = null;
        public string? Os { get; set; } = null;
        public required string TokenId { get; set; }
        public Token? Token { get; set; } = null;

    }
}
