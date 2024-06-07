using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Lab1_
{
    internal class SymmetricEncryption
    {
        public static (string encryptedText, string iv) Encrypt(string text, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetKeyBytes(key, aesAlg.KeySize / 8); // Ensure key is the correct size
                aesAlg.GenerateIV(); // Generate new IV

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                        byte[] encrypted = msEncrypt.ToArray();
                        return (Convert.ToBase64String(encrypted), Convert.ToBase64String(aesAlg.IV));
                    }
                }
            }
        }

        public static string Decrypt(string encryptedText, string key, string iv)
        {
            byte[] cipherText = Convert.FromBase64String(encryptedText);
            byte[] ivBytes = Convert.FromBase64String(iv);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetKeyBytes(key, aesAlg.KeySize / 8); // Ensure key is the correct size
                aesAlg.IV = ivBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        private static byte[] GetKeyBytes(string key, int size)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length == size)
            {
                return keyBytes;
            }

            byte[] resizedKey = new byte[size];
            Array.Copy(keyBytes, resizedKey, Math.Min(keyBytes.Length, resizedKey.Length));
            return resizedKey;
        }
    }
}
