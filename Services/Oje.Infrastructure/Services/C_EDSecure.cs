﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace Oje.Infrastructure.Services
{
    public class C_EDSecure
    {
        public byte[] Key = {
            12, 83, 27, 128, 81, 35, 104, 154, 70, 68, 5, 193, 72, 11, 23, 110, 222, 53, 239, 4, 8, 73, 60, 30, 120, 201, 217, 69, 190, 9, 5, 2
        };
        public byte[] Vector = {
           42, 22, 64, 100, 180, 172, 241, 36, 82, 205, 142, 175, 14, 63, 25, 6
        };
        private readonly ICryptoTransform EncryptorTransform;
        private readonly ICryptoTransform DecryptorTransform;
        private readonly System.Text.UTF8Encoding UTFEncoder;
        static C_EDSecure todayInstance { get; set; }
        static string todayCacheTime { get; set; }

        public static C_EDSecure generateNewIfNeeded(byte[] key, byte[] value)
        {
            if(
                (todayCacheTime == null || todayInstance == null) || 
                (todayCacheTime != DateTime.Now.ToString("yyyy-MM-dd"))
              )
            {
                todayInstance = new C_EDSecure(key, value);
                todayCacheTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            return todayInstance;
        }

        public C_EDSecure()
        {
            //This is our encryption method
            using (Aes rm = Aes.Create("AesManaged"))
            {
                rm.Mode = CipherMode.CBC;
                //rm.KeySize =128 ;
                //rm.BlockSize = 256;
                //Create an encryptor and a decryptor using our encryption method, key, and vector.
                EncryptorTransform = rm.CreateEncryptor(this.Key, this.Vector);
                DecryptorTransform = rm.CreateDecryptor(this.Key, this.Vector);
            }

            //Used to translate bytes to text and vice versa
            UTFEncoder = new System.Text.UTF8Encoding();
        }

        public C_EDSecure(byte[] arrKey, byte[] arrVector)
        {
            using (Aes rm = Aes.Create("AesManaged"))
            {
                rm.Mode = CipherMode.CBC;
                EncryptorTransform = rm.CreateEncryptor(arrKey, arrVector);
                DecryptorTransform = rm.CreateDecryptor(arrKey, arrVector);
            }

            //Used to translate bytes to text and vice versa
            UTFEncoder = new System.Text.UTF8Encoding();
        }

        /// -------------- Two Utility Methods (not used but may be useful) -----------
        /// Generates an encryption key.
        static public byte[] GenerateEncryptionKey()
        {
            //Generate a Key.
            using (Aes rm = Aes.Create("AesManaged"))
            {
                rm.GenerateKey();
                return rm.Key;
            }
        }

        /// Generates a unique encryption vector
        static public byte[] GenerateEncryptionVector()
        {
            //Generate a Vector
            using (Aes rm = Aes.Create("AesManaged"))
            {
                rm.GenerateIV();
                return rm.IV;
            }
        }


        /// ----------- The commonly used methods ------------------------------    
        /// Encrypt some text and return a string suitable for passing in a URL.
        public string EncryptToString(string TextValue)
        {
            return ByteArrToString(Encrypt(TextValue));
        }

        /// Encrypt some text and return an encrypted byte array.
        public byte[] Encrypt(string TextValue)
        {
            //Translates our text value into a byte array.
            Byte[] bytes = UTFEncoder.GetBytes(TextValue);

            //Used to stream the data in and out of the CryptoStream.
            MemoryStream memoryStream = new MemoryStream();

            /*
             * We will have to write the unencrypted bytes to the stream,
             * then read the encrypted result back from the stream.
             */
            #region Write the decrypted value to the encryption stream
            CryptoStream cs = new CryptoStream(memoryStream, EncryptorTransform, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();
            #endregion

            #region Read encrypted value back out of the stream
            memoryStream.Position = 0;
            byte[] encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);
            #endregion

            //Clean up.
            cs.Close();
            memoryStream.Close();

            return encrypted;
        }

        /// The other side: Decryption methods
        public string DecryptString(string EncryptedString)
        {
            return Decrypt(StrToByteArray(EncryptedString));
        }

        /// Decryption when working with byte arrays.    
        public string Decrypt(byte[] EncryptedValue)
        {
            #region Write the encrypted value to the decryption stream
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
            decryptStream.Write(EncryptedValue, 0, EncryptedValue.Length);
            decryptStream.FlushFinalBlock();
            #endregion

            #region Read the decrypted value from the stream.
            encryptedStream.Position = 0;
            Byte[] decryptedBytes = new Byte[encryptedStream.Length];
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            encryptedStream.Close();
            #endregion
            return UTFEncoder.GetString(decryptedBytes);
        }

        /// Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
        //      System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        //      return encoding.GetBytes(str);
        // However, this results in character values that cannot be passed in a URL.  So, instead, I just
        // lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
        public byte[] StrToByteArray(string str)
        {
            if (str.Length == 0)
                throw new Exception("Invalid string value in StrToByteArray");

            byte val;
            byte[] byteArr = new byte[str.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                val = byte.Parse(str.Substring(i, 3));
                byteArr[j++] = val;
                i += 3;
            }
            while (i < str.Length);
            return byteArr;
        }

        // Same comment as above.  Normally the conversion would use an ASCII encoding in the other direction:
        //      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        //      return enc.GetString(byteArr);    
        public string ByteArrToString(byte[] byteArr)
        {
            byte val;
            string tempStr = "";
            for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
            {
                val = byteArr[i];
                if (val < (byte)10)
                    tempStr += "00" + val.ToString();
                else if (val < (byte)100)
                    tempStr += "0" + val.ToString();
                else
                    tempStr += val.ToString();
            }
            return tempStr;
        }
    }
}
