using System;
using System.IO;
using System.Security.Cryptography;

namespace Framework.Tools
{
    public abstract class EncryptionUtility
    {
        private const int KeySize = 256;
        private const int BlockSize = 128;

        public static (byte[] Key, byte[] IV) GenerateKeyAndIv()
        {
            // using (Aes aesAlg = Aes.Create())
            // {
            //     aesAlg.KeySize = KeySize;
            //     aesAlg.BlockSize = BlockSize;
            //     aesAlg.GenerateKey();
            //     aesAlg.GenerateIV();
            //
            //     return (aesAlg.Key, aesAlg.IV);
            // }
            // 手动定义一个256位Key和128位IV
            byte[] customKey = new byte[32] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 
                0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 
                0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 
                0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20 };

            byte[] customIV = new byte[16] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 
                0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };

            return (customKey, customIV);
        }

        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = KeySize;
                aesAlg.BlockSize = BlockSize;
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor();

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        public static byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = KeySize;
                aesAlg.BlockSize = BlockSize;
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor();

                using (MemoryStream msDecrypt = new MemoryStream())
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(encryptedData, 0, encryptedData.Length);
                    }
                    return msDecrypt.ToArray();
                }
            }
        }
    }
}
