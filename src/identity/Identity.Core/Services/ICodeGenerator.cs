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
    }
}
