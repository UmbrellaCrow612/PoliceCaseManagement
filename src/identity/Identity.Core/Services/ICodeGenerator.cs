namespace Identity.Core.Services
{
    /// <summary>
    /// Defines the business contract for handling all MFA (multi-factor-authentication) code generation and other code generation logic
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// Generates a random six digit number
        /// </summary>
        /// <returns></returns>
        string GenerateSixDigitCode();

        /// <summary>
        /// Generates a random unique code number and string combo
        /// </summary>
        /// <returns></returns>
        string GenerateUnique();

        /// <summary>
        /// Generates a random Base32 secret suitable for TOTP
        /// </summary>
        /// <param name="length">Length of the secret in characters (default 32)</param>
        /// <returns>Base32-encoded secret</returns>
        string GenerateBase32Secret(int length = 32);
    }
}
