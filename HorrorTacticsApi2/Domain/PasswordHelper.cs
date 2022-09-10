using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace HorrorTacticsApi2.Domain
{
    public class PasswordHelper
    {
        public byte[] GenerateHash(string password, byte[] salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Cannot be null or empty", nameof(password));

            return KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, Constants.PasswordIterations, Constants.PasswordSize);
        }

        public byte[] GenerateSalt()
        {
            using var provider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[Constants.PasswordSaltSize];
            provider.GetNonZeroBytes(salt);

            return salt;
        }
    }
}
