using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace carWashManagement
{
    internal class PasswordHelper
    {
        private static readonly string Salt = "AutoSpaFixedSalt2024";

        public static string Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                return password;

            using (SHA256 sha = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password + Salt);
                byte[] hashBytes = sha.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
