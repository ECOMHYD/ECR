using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper
{
    private static readonly string Key = "F3@kL8xPz1BvR7yQw2NtD9mH5XsC0GpV"; // Ensure this is a 32-char key for AES-256

    public static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.GenerateIV();
            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cs))
                        {
                            writer.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }

    public static string Decrypt(string encryptedText)
    {
        byte[] buffer = Convert.FromBase64String(encryptedText);
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            byte[] iv = new byte[aes.IV.Length];
            Array.Copy(buffer, 0, iv, 0, iv.Length);
            aes.IV = iv;
            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                using (var ms = new MemoryStream(buffer, iv.Length, buffer.Length - iv.Length))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cs))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
