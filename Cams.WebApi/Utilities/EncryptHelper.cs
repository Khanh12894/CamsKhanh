using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XichLip.WebApi.Utilities
{
    public class EncryptHelper
    {
        #region Variables

        private static Byte[] KEY_192 =
            new byte[] { 42, 50, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 96, 155, 112, 2, 94, 11, 204, 119, 35, 184, 186 };

        private static Byte[] IV_192 =
            new byte[] { 55, 103, 245, 79, 36, 99, 167, 3, 42, 5, 62, 83, 184, 3, 209, 13, 145, 23, 200, 58, 173, 10, 121, 111 };

        #endregion Variables

        #region Public Methods

        #region Encrypt

        /// <summary>
        /// Encrypts the specified value
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            return TripleDESEncryption(value);
        }

        #endregion Encrypt

        #region Decrypt

        /// <summary>
        /// Decrypts the specified value
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns></returns>
        public static string Decrypt(string value)
        {
            return TripleDESDecryption(value);
        }

        #endregion Decrypt

        #endregion Public Methods

        #region Private Methods

        #region TripleDESEcryption

        /// <summary>
        /// Triples the DES encryption.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns></returns>
        private static string TripleDESEncryption(string value)
        {
            string outStr = "";
            try
            {
                if (!value.Equals(""))
                {
                    TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs =
                        new CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_192, IV_192), CryptoStreamMode.Write);
                    StreamWriter sw = new StreamWriter(cs);
                    sw.Write(value);
                    sw.Flush();
                    cs.FlushFinalBlock();
                    ms.Flush();
                    //outStr = Convert.ToBase64CharArray(ms.GetBuffer(), 0, (int)ms.Length);
                    outStr = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

                    // TODO: added by SonP (15 Aug 08)
                    outStr = outStr.Replace("+", "@");
                }
            }
            catch { }
            return outStr;
        }

        #endregion TripleDESEcryption

        #region TripleDESDecryption

        /// <summary>
        /// Triples the DES decryption.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns></returns>
        private static string TripleDESDecryption(string value)
        {
            string outStr = string.Empty;
            try
            {
                if (!value.Equals(string.Empty))
                {
                    // TODO: added by SonP (15 Aug 08)
                    value = value.Replace("@", "+");

                    TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider();
                    //byte[] buffer = Convert.FromBase64CharArray(value.ToCharArray(), 0, value.Length);
                    byte[] buffer = Convert.FromBase64String(value);
                    MemoryStream ms = new MemoryStream(buffer);
                    CryptoStream cs =
                        new CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_192, IV_192), CryptoStreamMode.Read);
                    StreamReader sr = new StreamReader(cs);
                    outStr = sr.ReadToEnd().ToString();
                }
            }
            catch { }
            return outStr;
        }

        #endregion TripleDESDecryption

        #endregion Private Methods
    }
}
