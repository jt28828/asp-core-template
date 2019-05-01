using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DotnetCoreWebApiTemplate.Utilities
{
    /// <summary>
    /// Basic utilities used for cryptography functions
    /// </summary>
    internal class CryptoUtils
    {
        /// <summary> Generates a cryptographically secure 128 bit salt </summary>
        internal static string GenerateSalt()
        {
            // Generate a 128-bit salt using a secure Random Number Generator
            byte[] salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Returns the salt as a string
            return Convert.ToBase64String(salt);
        }

        /// <summary> Returns the Hash of the given password using HMAC256 with 15000 iterations </summary>
        /// <param name="password"> The Password to Hash </param>
        /// <param name="salt"> The Unique Salt to include in the Hash </param>
        /// <returns> The Hashed Password </returns>
        internal static string HashPassword(string password, string salt)
        {
            byte[] convertedSalt = Convert.FromBase64String(salt);

            // Use HMACSHA256 to generate a Hash
            byte[] hashedBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: convertedSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 15000,
                numBytesRequested: 64);

            return Convert.ToBase64String(hashedBytes);
        }

        /// <summary> Verifies that the given plaintext password matches its hashed variant </summary>
        /// <param name="plaintextPassword"> The password in plaintext format </param>
        /// <param name="salt"> The user's unique salt </param>
        /// <param name="hashedPassword"> The Hashed password to compare to </param>
        /// <returns> True or false depending on match </returns>
        internal static bool VerifyPassword(string plaintextPassword, string salt, string hashedPassword)
        {
            string hashedPlaintext = HashPassword(plaintextPassword, salt);
            return hashedPlaintext == hashedPassword;
        }
    }
}