using System;
using System.Security.Cryptography;
using System.Text;

namespace LoansApi.Services
{
    public interface IHashService
    {
        public  string GenerateSalt();
        public  string GenerateHash(string password, string salt);
    }
    public class HashPasswordService : IHashService
    {
        public  string GenerateSalt()
        {
            var random = new RNGCryptoServiceProvider();
            var max_length = 32;
            var salt = new byte[max_length];

            random.GetNonZeroBytes(salt);

            return Convert.ToBase64String(salt);
        }

        public string GenerateHash(string password, string salt)
        {
            // SHA512 is disposable by inheritance.  
            using var sha256 = SHA256.Create();
            // Send a sample text to hash.  
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            // Get the hashed string.  
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }


    }
}
