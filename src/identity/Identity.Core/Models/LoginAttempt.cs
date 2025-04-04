﻿namespace Identity.Core.Models
{
    public class LoginAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public required string IpAddress { get; set; }
        public required string UserAgent { get; set; }
        public LoginStatus Status { get; set; } = LoginStatus.FAILED;
        public string? FailureReason { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime ExpiresAt { get; set; }

        public ICollection<TwoFactorSmsAttempt> TwoFactorSmsAttempts { get; set; } = [];
        public ICollection<TwoFactorEmailAttempt> TwoFactorEmailAttempts { get; set; } = [];

        public bool IsValid()
        {
            if (Status != LoginStatus.TwoFactorAuthenticationReached)
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
            Status = LoginStatus.SUCCESS;
        }
    }

    public enum LoginStatus
    {
        SUCCESS = 0,
        FAILED = 1,
        BLOCKED = 2,
        TwoFactorAuthenticationReached = 3,
    }
}

