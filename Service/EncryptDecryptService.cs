using System.Security.Cryptography;
using System.Text;

namespace ConnectionStringSecureAPI.Service
{
    public static class EncryptDecryptService
    {
        private static readonly string Key = "1234567890123456"; // Use a strong and secure key

       // private static readonly string Key = GenerateRandomKey(); // Use a strong and secure key

        private static string GenerateRandomKey()
        {
            const int keyLength = 16; // Length of the key
            var randomKey = new StringBuilder(keyLength);

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[keyLength];

                // Fill the byte array with secure random numbers
                rng.GetBytes(randomBytes);

                foreach (var b in randomBytes)
                {
                    // Ensure the key contains numeric characters
                    randomKey.Append((b % 10).ToString()); // Get numbers between 0-9
                }
            }

            return randomKey.ToString();
        }


        public static string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = new byte[16];

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                        sw.Write(plainText);

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = new byte[16];

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                    return sr.ReadToEnd();
            }
        }

        public static bool IsEncrypted(string input)
        {
            // If the connection string starts with "Server=", it is decrypted
            if (input.StartsWith("Server=", StringComparison.OrdinalIgnoreCase))
            {
                return false; // Connection is decrypted
            }

            // Otherwise, assume the connection string is encrypted
            return true; // Connection is encrypted
        }
    }
}
