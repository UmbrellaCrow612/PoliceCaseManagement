using System.Security.Cryptography;

namespace PoliceCaseManagement.Shared.Utils
{
    /// <summary>
    /// PoliceCaseManagement custom password hasher used throughout the app.
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32; // 256 bits
        private const int Iterations = 100000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
        private const char Delimiter = ':';

        public static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

            return string.Join(
                Delimiter,
                Convert.ToBase64String(hash),
                Convert.ToBase64String(salt),
                Iterations,
                Algorithm
            );
        }

        public static bool VerifyPassword(string passwordHash, string inputPassword)
        {
            var elements = passwordHash.Split(Delimiter);
            if (elements.Length != 4) return false;

            var hash = Convert.FromBase64String(elements[0]);
            var salt = Convert.FromBase64String(elements[1]);
            var iterations = int.Parse(elements[2]);
            var algorithm = new HashAlgorithmName(elements[3]);

            var inputHash = Rfc2898DeriveBytes.Pbkdf2(
                inputPassword,
                salt,
                iterations,
                algorithm,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
