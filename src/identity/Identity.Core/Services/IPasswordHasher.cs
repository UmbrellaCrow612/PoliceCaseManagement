namespace Identity.Core.Services
{
    /// <summary>
    /// Provides functionality to hash and verify passwords.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hashes the specified password using a cryptographically secure algorithm.
        /// The resulting string contains both the salt and the hash, encoded as Base64.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <returns>A Base64 string containing both the salt and the hash.</returns>
        string Hash(string password);

        /// <summary>
        /// Verifies whether the provided plain text password matches the stored hashed password.
        /// The salt is extracted automatically from the stored hash.
        /// </summary>
        /// <param name="hashedPassword">The previously stored hashed password (Base64 string containing salt + hash).</param>
        /// <param name="plainTextPassword">The plain text password to verify.</param>
        /// <returns><c>true</c> if the password is valid; otherwise, <c>false</c>.</returns>
        bool Verify(string hashedPassword, string plainTextPassword);
    }
}
