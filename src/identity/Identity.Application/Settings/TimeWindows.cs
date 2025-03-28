﻿using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Settings
{
    /// <summary>
    /// Configures time windows for critical identity-related actions, 
    /// controlling the duration of temporary access and reset tokens.
    /// </summary>
    /// <remarks>
    /// These settings define how long sensitive operations remain valid and 
    /// help prevent potential security vulnerabilities by limiting token lifespans.
    /// Values are typically specified in minutes.
    /// </remarks>
    public class TimeWindows
    {
        /// <summary>
        /// Duration for which a password reset token remains valid.
        /// Prevents indefinite password reset attempts.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ResetPasswordTime must be greater than 0.")]
        public required int ResetPasswordTime { get; set; }

        /// <summary>
        /// Time window for email confirmation tokens.
        /// Limits the time users have to confirm their email address.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "EmailConfirmationTime must be greater than 0.")]
        public required int EmailConfirmationTime { get; set; }

        /// <summary>
        /// Duration of device challenge authentication tokens.
        /// Restricts the window for additional device verification.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "DeviceChallengeTime must be greater than 0.")]
        public required int DeviceChallengeTime { get; set; }

        /// <summary>
        /// Time allowed for phone number confirmation process.
        /// Ensures timely verification of contact information.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "PhoneConfirmationTime must be greater than 0.")]
        public required int PhoneConfirmationTime { get; set; }

        /// <summary>
        /// Time allowed for a two factor code to be valid for.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TwoFactorSmsTime must be greater than 0.")]
        public required int TwoFactorSmsTime { get; set; }

        /// <summary>
        /// Time allowed for a two factor code to be valid for.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TwoFactorEmailTime must be greater than 0.")]
        public required int TwoFactorEmailTime { get; set; }

        /// <summary>
        /// How long a login attempt is alive for it to be used to issue 2FA codes for.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "LoginLifetime must be greater than 0.")]
        public required int LoginLifetime { get; set; }
    }
}