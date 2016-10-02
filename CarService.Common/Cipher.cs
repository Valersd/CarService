using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CarService.Common
{
    public static class Cipher
    {
        private const string Salt = "T8JKMV3C";

        public static string Encrypt(string text)
        {
            byte[] key = { };
            byte[] IV = { 0x32, 0x41, 0x54, 0x67, 0x73, 0x21, 0x47, 0x19 };
            MemoryStream ms = null;

            try
            {
                key = Encoding.UTF8.GetBytes(Salt);
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                DESCryptoServiceProvider dcp = new DESCryptoServiceProvider();
                ICryptoTransform ict = dcp.CreateEncryptor(key, IV);
                ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, ict, CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
            }
            catch (Exception)
            {
                return text;
            }
            return Convert.ToBase64String(ms.ToArray()).Replace('+', '_');
        }

        public static string Decrypt(string text)
        {
            byte[] key = { };
            byte[] IV = { 0x32, 0x41, 0x54, 0x67, 0x73, 0x21, 0x47, 0x19 };
            MemoryStream ms = null;

            try
            {
                key = Encoding.UTF8.GetBytes(Salt);
                byte[] bytes = new byte[text.Length];
                bytes = Convert.FromBase64String(text.Replace('_', '+'));
                DESCryptoServiceProvider dcp = new DESCryptoServiceProvider();
                ICryptoTransform ict = dcp.CreateDecryptor(key, IV);
                ms = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(ms, ict, CryptoStreamMode.Write);
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
            }
            catch (Exception)
            {
                return text;
            }
            Encoding en = Encoding.UTF8;
            return en.GetString(ms.ToArray());
        }
    }
}
