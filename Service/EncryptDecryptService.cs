using System.Security.Cryptography;
using System.Text;

namespace ConnectionStringSecureAPI.Service
{
    public static class EncryptDecryptService
    {
        private static readonly string Key = "1234567890123456"; // Use a strong and secure key

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
