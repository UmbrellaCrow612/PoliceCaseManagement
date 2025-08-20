using Identity.Core.Services;
using System.Security.Cryptography;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Implementation of <see cref="Core.Services.IPasswordHasher"/>
    /// </summary>
    public class PasswordHasherImpl : IPasswordHasher
    {
        public string Hash(string password)
        {
            // Generate random 16-byte salt
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // Hash with PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            // Combine salt + hash
            byte[] hashBytes = new byte[salt.Length + hash.Length];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, hashBytes, salt.Length, hash.Length);

            // Encode as Base64
            return Convert.ToBase64String(hashBytes);
        }

        public bool Verify(string hashedPassword, string plainTextPassword)
        {
            // Decode Base64 back to byte[]
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Extract salt (first 16 bytes) and hash (next 32 bytes)
            byte[] salt = new byte[16];
            byte[] storedSubHash = new byte[32];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);
            Buffer.BlockCopy(hashBytes, 16, storedSubHash, 0, 32);

            // Recompute hash with extracted salt
            var pbkdf2 = new Rfc2898DeriveBytes(plainTextPassword, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] computedHash = pbkdf2.GetBytes(32);

            // Compare securely
            return CryptographicOperations.FixedTimeEquals(storedSubHash, computedHash);
        }
    }
}
