using Identity.Core.Services;
using System.Security.Cryptography;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="ICodeGenerator"/> - test this, as well when using it else where only use the <see cref="ICodeGenerator"/>
    /// interface not this class
    /// </summary>
    public class CodeGeneratorImpl : ICodeGenerator
    {
        public string GenerateSixDigitCode()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[4]; // 4 bytes = 32 bits
            rng.GetBytes(bytes);
            // Convert bytes to an integer
            int randomNumber = Math.Abs(BitConverter.ToInt32(bytes, 0));
            // Limit to 6 digits
            return (randomNumber % 1000000).ToString("D6");
        }
    }
}
