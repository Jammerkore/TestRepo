using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;


namespace MIDRetail.Encryption
{
    public class MIDEncryption
    {
        public string cEncryptKey = "m1DreT01";
        // Begin TT#580-MD - JSmith - Error During One-Click Upgrade
        public string cOldEncryptTag = "##MID##";
        public string cEncryptTag = "##MID2##";
        private const int cOffset = 15;
        // End TT#580-MD - JSmith - Error During One-Click Upgrade

        // Begin TT#580-MD - JSmith - Error During One-Click Upgrade
        //public string Encrypt(string originalString)
        //{
        //    if (String.IsNullOrEmpty(originalString))
        //    {
        //        return string.Empty;
        //        //throw new ArgumentNullException
        //        //       ("The string which needs to be encrypted can not be null.");
        //    }
        //    byte[] bytes = ASCIIEncoding.ASCII.GetBytes(cEncryptKey);
        //    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        //    MemoryStream memoryStream = new MemoryStream();
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream,
        //        cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
        //    StreamWriter writer = new StreamWriter(cryptoStream);
        //    writer.Write(originalString);
        //    writer.Flush();
        //    cryptoStream.FlushFinalBlock();
        //    writer.Flush();
        //    return cEncryptTag + Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        //}

        ///// <summary>
        ///// Decrypt a crypted string.
        ///// </summary>
        ///// <param name="cryptedString">The crypted string.</param>
        ///// <returns>The decrypted string.</returns>
        ///// <exception cref="ArgumentNullException">This exception will be thrown 
        ///// when the crypted string is null or empty.</exception>
        //public string Decrypt(string cryptedString)
        //{
        //    if (String.IsNullOrEmpty(cryptedString))
        //    {
        //        return string.Empty;
        //        //throw new ArgumentNullException
        //        //   ("The string which needs to be decrypted can not be null.");
        //    }
        //    // not encrypted so just return
        //    if (!cryptedString.StartsWith(cEncryptTag))
        //    {
        //        return cryptedString;
        //    }
        //    // remove tag
        //    cryptedString = cryptedString.Replace(cEncryptTag, null);
        //    // decompress
        //    byte[] bytes = ASCIIEncoding.ASCII.GetBytes(cEncryptKey);
        //    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        //    MemoryStream memoryStream = new MemoryStream
        //            (Convert.FromBase64String(cryptedString));
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream,
        //        cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
        //    StreamReader reader = new StreamReader(cryptoStream);
        //    return reader.ReadToEnd();
        //}

        public string Encrypt(string originalString)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                return string.Empty;
            }

            // already encrypted
            if (originalString.StartsWith(cOldEncryptTag))
            {
                originalString = Decrypt(originalString);
            }
            else if (originalString.StartsWith(cEncryptTag))
            {
                return originalString;
            }
            
            string encrypt = "";
            for (int iChar = 0; iChar < originalString.Length; iChar++)
            {
                encrypt += (char)(originalString[iChar] + cOffset);
            }

            return cEncryptTag + encrypt;
        }

        /// <summary>
        /// Decrypt a crypted string.
        /// </summary>
        /// <param name="cryptedString">The crypted string.</param>
        /// <returns>The decrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be thrown 
        /// when the crypted string is null or empty.</exception>
        public string Decrypt(string cryptedString)
        {
            string decrypt = "";
            if (String.IsNullOrEmpty(cryptedString))
            {
                return string.Empty;
            }
            // Begin TT#580-MD - JSmith - Error During One-Click Upgrade
            //// not encrypted so just return
            //if (!cryptedString.StartsWith(cEncryptTag))
            //{
            //    return cryptedString;
            //}
            // if old tag use old decryption
            if (cryptedString.StartsWith(cOldEncryptTag))
            {
                return OldDecrypt(cryptedString);
            }
            // not encrypted so just return
            else if (!cryptedString.StartsWith(cEncryptTag))
            {
                return cryptedString;
            }
            // End TT#580-MD - JSmith - Error During One-Click Upgrade

            // remove tag
            cryptedString = cryptedString.Replace(cEncryptTag, null);
            for (int iChar = 0; iChar < cryptedString.Length; iChar++)
            {
                decrypt += (char)(cryptedString[iChar] - cOffset);
            }

            return decrypt;
        }

        public string OldDecrypt(string cryptedString)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                return string.Empty;
            }
            // not encrypted so just return
            if (!cryptedString.StartsWith(cOldEncryptTag))
            {
                return cryptedString;
            }
            // remove tag
            cryptedString = cryptedString.Replace(cOldEncryptTag, null);
            // decompress
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(cEncryptKey);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream
                    (Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
        // End TT#580-MD - JSmith - Error During One-Click Upgrade
    }
}
