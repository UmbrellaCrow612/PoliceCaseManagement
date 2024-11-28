namespace Email.Service.Settings
{
    public class EmailSettings
    {
        public required string SenderEmail { get; set; }
        public required string SenderPassword { get; set; }
        public required string SmtpServer { get; set; }
        public required int SmtpPort { get; set; }
    }
}
