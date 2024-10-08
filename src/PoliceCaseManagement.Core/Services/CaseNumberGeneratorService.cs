using System.Security.Cryptography;

namespace PoliceCaseManagement.Core.Services
{
    public static class CaseNumberGeneratorService
    {
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        /// <summary>
        /// Method to invoke to generate a unique case number
        /// </summary>
        /// <returns>String case number</returns>
        public static string GenerateCaseNumber()
        {
            // Get the current year
            var year = DateTime.UtcNow.Year;

            // Generate a random 8-digit number
            var randomNumber = GenerateRandomNumber(8);

            // Format the case number
            var caseNumber = $"CA-{year}-{randomNumber:D8}";
            return caseNumber;
        }

        private static int GenerateRandomNumber(int digits)
        {
            byte[] randomBytes = new byte[4];
            rng.GetBytes(randomBytes);
            int randomInt = BitConverter.ToInt32(randomBytes, 0);
            return Math.Abs(randomInt) % (int)Math.Pow(10, digits);
        }
    }
}
