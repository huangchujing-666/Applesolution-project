using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.Modules.Utility
{
    public static class CryptographyManager
    {
        // AES 256 bit Encryption
        // CBC Mode
        // Padding Mode = PKCS7
        // Key Size = 256 bits
        // Block Size = 16 bytes (128 bits is standard in Rijndael)
        // return Base64 String
        public static string EncryptAES256_CBC(string key, string iv, string plainText)
        {
            var resultString = "";

            try
            {
                var myRijndael = new RijndaelManaged()
                {
                    Padding = PaddingMode.PKCS7,
                    Mode = CipherMode.CBC,
                    KeySize = 256,  //bits
                    BlockSize = 128,  //bits
                };

                var plainTextBytes = Encoding.ASCII.GetBytes(plainText);
                //System.Diagnostics.Debug.WriteLine(BitConverter.ToString(plainTextBytes).Replace("-", ","));
                //System.Diagnostics.Debug.WriteLine(Convert.ToBase64String(plainTextBytes));
                //foreach (var a in plainTextBytes)
                //{
                //    System.Diagnostics.Debug.Write(a.ToString() + ", ");
                //}
                //System.Diagnostics.Debug.WriteLine("");

                // key = "QHTLkl*ka%k!)825as*a2svgi2qpc%(!"; //QHTLkl*ka%k!)825as*a2svgi2qpc%(!, QHTLklAkaBkCD825asEa2svgi2qpcFPA
                var keyBytes = Encoding.ASCII.GetBytes(key);
                //System.Diagnostics.Debug.WriteLine(BitConverter.ToString(keyBytes).Replace("-", ","));
                //System.Diagnostics.Debug.WriteLine(Convert.ToBase64String(keyBytes));
                //foreach (var a in key)
                //{
                //    System.Diagnostics.Debug.Write(a.ToString() + ", ");
                //}
                //System.Diagnostics.Debug.WriteLine("");

                // iv = "cyjmna*^51AYn230"  //"abcdef123456abcd");
                var ivBytes = Encoding.ASCII.GetBytes(iv);

                var encryptor = myRijndael.CreateEncryptor(keyBytes, ivBytes);
                var msEncrypt = new MemoryStream();
                var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

                csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
                csEncrypt.FlushFinalBlock();

                var encryptedData = msEncrypt.ToArray();
                //System.Diagnostics.Debug.WriteLine(BitConverter.ToString(encryptedData).Replace("-", ","));
                //System.Diagnostics.Debug.WriteLine(Convert.ToBase64String(encryptedData));
                //foreach (var a in encryptedData)
                //{
                //    System.Diagnostics.Debug.Write(a.ToString() + ", ");
                //}
                //System.Diagnostics.Debug.WriteLine("");

                resultString = Convert.ToBase64String(encryptedData);
            }
            catch (Exception e)
            {
                resultString = "";
                System.Diagnostics.Debug.WriteLine("Exception: " + e.ToString());
            }
            return resultString;
        }

        // AES 256 bit Encryption
        // ECB Mode
        // Padding Mode = PKCS7
        // Key Size = 256 bits
        // Block Size = 16 bytes (128 bits is standard in Rijndael)
        // return Base64 String
        public static string EncryptAES256_ECB(string key, string plainText)
        {
            var resultString = "";

            try
            {
                var myRijndael = new RijndaelManaged()
                {
                    Padding = PaddingMode.PKCS7,
                    Mode = CipherMode.ECB,
                    KeySize = 256,  //bits
                    BlockSize = 128,  //bits
                };

                var plainTextBytes = Encoding.ASCII.GetBytes(plainText);

                var keyBytes = Encoding.ASCII.GetBytes(key);
                var ivBytes = Encoding.ASCII.GetBytes("");  // no need in ECB mode

                var encryptor = myRijndael.CreateEncryptor(keyBytes, ivBytes);
                var msEncrypt = new MemoryStream();
                var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

                csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
                csEncrypt.FlushFinalBlock();

                var encryptedData = msEncrypt.ToArray();

                resultString = Convert.ToBase64String(encryptedData);
            }
            catch (Exception e)
            {
                resultString = "";
                System.Diagnostics.Debug.WriteLine("Exception: " + e.ToString());
            }

            return resultString;
        }

        // AES 256 bit Decryption on Base64 String
        // CBC Mode
        // Padding Mode = PKCS7
        // Key Size = 256 bits
        // Block Size = 16 bytes (128 bits is standard in Rijndael)
        public static string DecryptAES256_CBC(string key, string iv, string encryptedString)
        {
            var resultString = "";

            try
            {
                var myRijndael = new RijndaelManaged()
                {
                    Padding = PaddingMode.PKCS7,
                    Mode = CipherMode.CBC,
                    KeySize = 256, //bits
                    BlockSize = 128 //bits
                };

                var keyBytes = Encoding.ASCII.GetBytes(key);
                var ivBytes = Encoding.ASCII.GetBytes(iv);

                var decryptor = myRijndael.CreateDecryptor(keyBytes, ivBytes);

                var sEncrypted = Convert.FromBase64String(encryptedString);

                var fromEncrypt = new byte[sEncrypted.Length];

                var msDecrypt = new MemoryStream(sEncrypted);
                var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                resultString = (Encoding.ASCII.GetString(fromEncrypt).TrimEnd(new Char[] { '\0' }));  // also remove cryptography Padding
            }
            catch (Exception e)
            {
                resultString = "";
                System.Diagnostics.Debug.WriteLine("Exception: " + e.ToString());
            }

            return resultString;
        }
    }
}
