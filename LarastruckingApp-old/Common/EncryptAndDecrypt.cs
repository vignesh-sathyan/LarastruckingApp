using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LarastruckingApp.Common
{
    public class EncryptAndDecrypt
    {
        public static string Encrypt(string plainText)
        {
            DESCryptoServiceProvider des = null;
            MemoryStream mStream = null;
            try
            {

           
            string key = "jdsg432387#";
            byte[] EncryptKey = { };
            byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            EncryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
             des = new DESCryptoServiceProvider();
            byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
             mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                mStream.Dispose();
                des.Dispose();
            }
        }

        #region Funtion to Decrypt ID(from javascript)
        public string DecryptDES(string encryptedText)
        {          

            #region with date
           
            encryptedText = encryptedText.Remove(0, 18);
            encryptedText = encryptedText.Remove(encryptedText.Length - 15);
            return encryptedText;
            #endregion
            
          
          
        }

        public string dec(string x)
        {
           // string rawtext = string.Empty;
            switch (x)
            {
                case "=":
                    return "0";

                case "i":
                    return "1";

                case "B":
                    return "2";

                case ">":
                    return "3";

                case "j":
                    return "4";

                case "X":
                    return "5";

                case "$":
                    return "6";

                case "Q":
                    return "7";

                case "Y":
                    return "8";

                case "Z":
                    return "9";

            }
            return null;
        }
             
        #endregion

       
    }
}