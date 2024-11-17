namespace Email.Service.Settings
{
    public class EmailOptions
    {
        public required string SmtpServer { get; set; } 
        public int SmtpPort { get; set; }
        public required string Username { get; set; } 
        public required string Password { get; set; }
        public bool UseSSL { get; set; }
        public required string FromEmail { get; set; }
        public required string FromName { get; set; } 
    }
}
