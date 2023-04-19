using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BaseSource.Application.Utilities.Cryptography
{
    public static class StringCipher
    {
        private const string privateKey = "SCSSolutions_REGAL-AllApplicationConfiguration@2025";

        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, bool isNetCore = false)
        {
            int num = 256;
            int bit = 32;
            if (isNetCore)
            {
                num = 128;
                bit = 16;
            }

            byte[] array = GenerateBitsOfRandomEntropy(bit);
            byte[] array2 = GenerateBitsOfRandomEntropy(bit);
            byte[] bytes = Encoding.Unicode.GetBytes(plainText);
            using Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("SCSSolutions_REGAL-AllApplicationConfiguration@2025", array, 1000);
            byte[] bytes2 = rfc2898DeriveBytes.GetBytes(num / 8);
            using RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.BlockSize = num;
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            using ICryptoTransform transform = rijndaelManaged.CreateEncryptor(bytes2, array2);
            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] inArray = array.Concat(array2).ToArray().Concat(memoryStream.ToArray())
                .ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(inArray);
        }

        public static string Decrypt(string cipherText, bool isNetCore = false)
        {
            int num = 256;
            if (isNetCore)
            {
                num = 128;
            }

            byte[] array = Convert.FromBase64String(cipherText);
            byte[] salt = array.Take(num / 8).ToArray();
            byte[] rgbIV = array.Skip(num / 8).Take(num / 8).ToArray();
            byte[] array2 = array.Skip(num / 8 * 2).Take(array.Length - num / 8 * 2).ToArray();
            using Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("SCSSolutions_REGAL-AllApplicationConfiguration@2025", salt, 1000);
            byte[] bytes = rfc2898DeriveBytes.GetBytes(num / 8);
            using RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.BlockSize = num;
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            using ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes, rgbIV);
            using MemoryStream memoryStream = new MemoryStream(array2);
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
            byte[] array3 = new byte[array2.Length];
            int count = cryptoStream.Read(array3, 0, array3.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.Unicode.GetString(array3, 0, count);
        }

        private static byte[] GenerateBitsOfRandomEntropy(int bit = 32)
        {
            byte[] array = new byte[bit];
            using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            rNGCryptoServiceProvider.GetBytes(array);
            return array;
        }
    }
}
