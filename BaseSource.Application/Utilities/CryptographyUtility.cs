
using BaseSource.Application.Utilities.Cryptography;

namespace BaseSource.Application.Utilities
{
    public static class CryptographyUtility
    {
        public static string Decrypt(string data)
        {
            string result;
            try
            {
                result = StringCipher.Decrypt(data, isNetCore: true);
            }
            catch
            {
                result = data;
            }
            return result;
        }
    }
}
