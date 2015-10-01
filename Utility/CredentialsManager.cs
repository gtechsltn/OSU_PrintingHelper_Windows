using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Utility
{
    public static class CredentialsManager
    {
        /*
         * Const fields.
         */
        private const string C_KEY = "SEVMTE9fV09STEQ=";
        private const string C_IV = "V0hBVF9ET19ZT1VfV0FOVA==";

        public static string EncryptText(string openText)
        {
            RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
            ICryptoTransform encryptor = rc2CSP.CreateEncryptor(Convert.FromBase64String(C_KEY), Convert.FromBase64String(C_IV));
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            byte[] toEncrypt = Encoding.Unicode.GetBytes(openText);

            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
            csEncrypt.FlushFinalBlock();

            byte[] encrypted = msEncrypt.ToArray();

            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptText(string encryptedText)
        {
            try
            {
                RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
                ICryptoTransform decryptor = rc2CSP.CreateDecryptor(Convert.FromBase64String(C_KEY), Convert.FromBase64String(C_IV));
                MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText));
                CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                List<Byte> bytes = new List<byte>();
                int b;
                do
                {
                    b = csDecrypt.ReadByte();
                    if (b != -1)
                    {
                        bytes.Add(Convert.ToByte(b));
                    }

                } while (b != -1);

                return Encoding.Unicode.GetString(bytes.ToArray());
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
