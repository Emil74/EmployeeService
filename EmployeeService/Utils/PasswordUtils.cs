using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.Utils
{
    public static class PasswordUtils
    {
        private const string SecretKey = "Fz8MguqN2DGWiD1ICvRxQ==";
        public static (string passwordSalt, string passwordHash) CreatePasswordHash(string password)
        {
            byte[] buffer = new byte[16];
            RNGCryptoServiceProvider rNGCryptoServiceProviderrsa = new RNGCryptoServiceProvider();
            rNGCryptoServiceProviderrsa.GetBytes(buffer);

            string passwordSalt = Convert.ToBase64String(buffer);
            string passwordHash = GetPasswordHash(password, passwordSalt);
            return (passwordHash, passwordSalt);
        }

        public static bool VerifyPassword(string password, string passwordSalt, string passwordHash)
        {
            return GetPasswordHash(password, passwordSalt) == passwordHash;
        }

        public static string GetPasswordHash(string password, string passwordSalt)
        {
            password = $"{password}~{passwordSalt}~{SecretKey}";
            byte[] buffer = Encoding.UTF8.GetBytes(password);

            SHA512 sHA512 = new SHA512Managed();
            byte[] passwordHash = sHA512.ComputeHash(buffer);
            return Convert.ToBase64String(passwordHash);
        }
    }
}
