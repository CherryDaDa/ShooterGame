using System;
using System.IO;
using System.Security.Cryptography;

namespace Framework.Tools
{
    public static class CryptoUtility
    {
        /// <summary>
        /// 使用AES加密字符串。
        /// </summary>
        /// <param name="plainText">待加密的明文字符串。</param>
        /// <param name="key">AES密钥，32字节（256位）。</param>
        /// <param name="iv">初始化向量，16字节（128位）。</param>
        /// <returns>加密后的Base64编码字符串。</returns>
        public static string Encrypt(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// 使用AES解密字符串。
        /// </summary>
        /// <param name="encryptedText">加密后的Base64编码字符串。</param>
        /// <param name="key">AES密钥，32字节（256位）。</param>
        /// <param name="iv">初始化向量，16字节（128位）。</param>
        /// <returns>解密后的明文字符串。</returns>
        public static string Decrypt(string encryptedText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 生成随机AES密钥。
        /// </summary>
        /// <returns>32字节的AES密钥。</returns>
        public static byte[] GenerateAesKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                return aes.Key;
            }
        }

        /// <summary>
        /// 生成随机初始化向量。
        /// </summary>
        /// <returns>16字节的初始化向量。</returns>
        public static byte[] GenerateAesIV()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateIV();
                return aes.IV;
            }
        }
    }
}
