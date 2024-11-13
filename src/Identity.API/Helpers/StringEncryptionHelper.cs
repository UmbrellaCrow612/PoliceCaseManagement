using System.Security.Cryptography;
using System.Text;

namespace Identity.API.Helpers
{
    public class StringEncryptionHelper(IConfiguration configuration)
    {
        private readonly string _encryptionKey = configuration["EncryptionKeys:StringEncryptionKey"] ?? throw new ApplicationException("StringEncryptionKey is null");

        /// <summary>
        /// Generates a random string of specified length.
        /// </summary>
        /// <param name="length">The length of the random string.</param>
        /// <returns>A random string.</returns>
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new StringBuilder(length);
            var byteArray = new byte[length];

            // Use the static RandomNumberGenerator method
            RandomNumberGenerator.Fill(byteArray);

            foreach (var b in byteArray)
            {
                result.Append(chars[b % chars.Length]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Hashes the provided plain text string using SHA256 with a key.
        /// </summary>
        /// <param name="plainText">The plain text to hash.</param>
        /// <returns>A Base64-encoded hash of the plain text.</returns>
        public string Hash(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentException("Plain text cannot be null or empty.");
            }

            using var sha256 = SHA256.Create();
            var combinedText = Encoding.UTF8.GetBytes(_encryptionKey + plainText);
            var hashBytes = sha256.ComputeHash(combinedText);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
