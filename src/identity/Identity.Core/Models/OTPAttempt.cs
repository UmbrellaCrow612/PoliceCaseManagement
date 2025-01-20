namespace Identity.Core.Models
{
    public class OTPAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; } = null;
        public required string Code { get; set; } // could hash and compare stuff but cant be bothered atm
        public required OTPMethod Method { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;


        public bool IsValid()
        {
            if (IsUsed || UsedAt.HasValue)
            {
                return false;
            }

            if (DateTime.UtcNow > ExpiresAt)
            {
                return false;
            }

            return true;
        }

        public void MarkUsed()
        {
            IsUsed = true;
            UsedAt = DateTime.UtcNow;
        }
    }

    public enum OTPMethod
    {
        Email = 0,
        Sms = 1,
    }

    public class OTPCreds
    {
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
