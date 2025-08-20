using Identity.Core.Models;
using Results.Abstractions;

namespace Identity.Core.Services
{
    /// <summary>
    /// Handles TOTP (Time Based One Time Passcodes)
    /// </summary>
    public interface ITotpService
    {
        /// <summary>
        /// Generate a TOTP (Time Based One Time Passcode) for a given user to scan
        /// </summary>
        /// <returns>Byte array of QR code image</returns>
        byte[] GenerateTotpQrCode(ApplicationUser user, string base32Secret, string issuer);

        /// <summary>
        /// Create a TOTP (Time Based One Time Passcode) for a given user and stores it
        /// </summary>
        /// <param name="user">The user to create it for</param>
        Task<TotpResult> GenerateTotp(ApplicationUser user);

        /// <summary>
        /// Resets and removes the current TOTP (Time Based One Time Passcodes) flag and secret - used when they want to generate a new TOTP and invalidate the old one
        /// </summary>
        /// <param name="user">The user to reset TOTP for</param>
        Task<IResult> ResetTotp(ApplicationUser user);
    }
  
    public class TotpResult : Result
    {
        public byte[] QrCodeBytes { get; set; } = [];
    }
}
