using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration; // for app settings
using System.Text; // for Encoding
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdobeReg.Utility
                  //namespace Microsoft.Extensions.DependencyInjection                  
{
    public class SlapCrypto : ISlapCrypto
    {
        private string keyName { get; set; }
        private string plaintext { get; set; }
        private string key { get; set; }
        private byte[] bkey { get; set; }
        private byte[] cipherText { get; set; }
        public IConfiguration Configuration { get; set; }

        //public SlapCrypto(IConfiguration configuration)
        //{
        //    Configuration = configuration; // added to inject configuration
        //}
        public SlapCrypto()
        {
        }
        public void setConfig(IConfiguration configuration)
        {
            Configuration = configuration; // added to inject configuration
        }
        /// <summary>
        /// Sets the key name for crypto key
        /// </summary>
        /// <returns><c>true</c>, if key was set, <c>false</c> otherwise.</returns>
        /// <param name="keyName">Key name.</param>
        public bool SetKey(string keyName)
        {
            if (keyName == null || keyName.Length == 0)
            {
                return false;
            }
            this.keyName = keyName;
            this.key = Configuration.GetSection(keyName).Value;
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            this.bkey = encoding.GetBytes(this.key);
            return true;
        }

        public string Decrypt(string content)
        {
            this.cipherText = Convert.FromBase64String(content);
          using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = this.bkey;
                aesAlg.IV = this.bkey;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(this.bkey
                                                                    , this.bkey);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(this.cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt
, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(
csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            this.plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                } // using (MemoryStream msDecrypt = new MemoryStream(cipherText)
            }
            return this.plaintext;
        } // Decrypt

        public string Encrypt(string text)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(text);
            byte[] _encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(this.bkey, this.bkey))
                {
                    aesAlg.BlockSize = 128;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Key = this.bkey;
                    aesAlg.IV = this.bkey;
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                       using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            csEncrypt.Write(clearBytes, 0, clearBytes.Length);
                            csEncrypt.FlushFinalBlock();
                            text = Convert.ToBase64String(msEncrypt.ToArray());
                            _encrypted = msEncrypt.ToArray();
                        }
                        return text;
                    } // using (MemoryStream msEncrypt
                } // using (ICryptoTransform encryptor
            } // using (Aes aesAlg
        } // Encrypt
    } // public class SlapCrypto
}
