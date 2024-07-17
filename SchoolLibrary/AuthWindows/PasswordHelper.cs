using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.AuthWindows
{
    //Используем метод шифрования паролей с использованием соль и SHA-256.
    public class PasswordHelper
    {
        public static byte[] GenerateSalt()
        {
            var salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
                return sha256.ComputeHash(saltedPassword);
            }
        }
    }
}
