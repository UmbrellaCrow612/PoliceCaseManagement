namespace Identity.Core.Services
{
    /// <summary>
    /// Defines the business contract for handling all MFA (multi-factor-authentication) code generation
    /// </summary>
    public interface ITwoFactorCodeGenerator
    {
        /// <summary>
        /// Generates a random six digit number
        /// </summary>
        /// <returns></returns>
        string GenerateSixDigitCode();
    }
}
