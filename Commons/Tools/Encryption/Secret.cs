using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Commons.Tools.Encryption
{
    public static class Secret
    {
        public static string EncryptAES(string data)
        {

            RijndaelManaged aes = new RijndaelManaged();
            byte[] bData = UTF8Encoding.UTF8.GetBytes(data);
            aes.Key = UTF8Encoding.UTF8.GetBytes(_aesKey);
            aes.IV = UTF8Encoding.UTF8.GetBytes(_aesKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform iCryptoTransform = aes.CreateEncryptor();
            byte[] bResult = iCryptoTransform.TransformFinalBlock(bData, 0, bData.Length);

            return Convert.ToBase64String(bResult); //返回base64加密;   
        }
        public static string DecryptAES(string data)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                byte[] bData = Convert.FromBase64String(data); //解密base64;   

                aes.Key = UTF8Encoding.UTF8.GetBytes(_aesKey);
                aes.IV = UTF8Encoding.UTF8.GetBytes(_aesKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform iCryptoTransform = aes.CreateDecryptor();
                byte[] bResult = iCryptoTransform.TransformFinalBlock(bData, 0, bData.Length);
                return Encoding.UTF8.GetString(bResult);
            }
            catch
            {
                throw;
            }
        }

        public static string _aesKey = "skiwatcs1011zes1";

    }
}
