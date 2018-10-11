using System;
using System.Security.Cryptography;
using Core.Entities;

namespace Auth
{
    public static class CredentialUtility
    {
        public static byte[] GenerateSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            return salt;
        }

        public static string HashPassword(string password)
        {
            var salt = GenerateSalt();

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public static bool IsValidPassword(UserEntity user, string password)
        {
            /* Fetch the stored value */
            string savedPasswordHash = user.PasswordHash;
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}
