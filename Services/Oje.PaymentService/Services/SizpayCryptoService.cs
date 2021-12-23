using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View;
using Oje.PaymentService.Models.View.SizPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Services
{
    public class SizpayCryptoService : ISizpayCryptoService
    {
        private CryptoModels _EnKeys;
        public CryptoModels Keys => _EnKeys;

        public static string SHA2(string prmKey, string prmPlainData)
        {
            byte[] varArrPlainData = default(byte[]);
            byte[] varArrCypherData = default(byte[]);
            byte[] varArrSHA2Key = default(byte[]);
            try
            {
                varArrSHA2Key = Encoding.UTF8.GetBytes(prmKey);
                varArrPlainData = Encoding.UTF8.GetBytes(prmPlainData);

                HMACSHA256 varSHA256 = new HMACSHA256(varArrSHA2Key);
                varArrCypherData = varSHA256.ComputeHash(varArrPlainData);

                return Convert.ToBase64String(varArrCypherData);
            }
            catch
            {
                return "";
            }
        }

        public SizpayCryptoEncryptResult Encrypt(string prmInput, CryptoModels keys)
        {

            _EnKeys = keys;

            string varLevel = string.Empty;
            clsAES varAes;
            string varSignData = string.Empty;
            try
            {
                varLevel = "0";
                varSignData = SHA2(Keys.FngrKey, prmInput);
                varLevel = "1";
                varAes = new clsAES(Keys.Key, Keys.IV);
                varLevel = "2";
                string tempX = "";
                varAes.Encrypt(string.Join(",", new string[] { prmInput, varSignData }), ref tempX);
                varLevel = "3";
                return new SizpayCryptoEncryptResult() { isSuccess = true, result = tempX, message = "" };
            }
            catch (Exception ex)
            {
                return new SizpayCryptoEncryptResult() { isSuccess = false, result = "", message = ex.InnerException == null ? ex.Message : ex.InnerException.Message };
            }
        }

        public bool Decrypt(string prmInput, ref string prmOutput, ref string prmStrErr)
        {
            string varLevel = string.Empty;
            clsAES varAes;
            string varSignData = string.Empty;
            List<string> varData = new List<string>();
            try
            {
                varLevel = "0";
                varAes = new clsAES(Keys.Key, Keys.IV);
                varLevel = "1";
                if (!varAes.Decrypt(prmInput, ref prmOutput))
                {
                    throw new Exception("خطا در رمزگشایی ");
                }
                varLevel = "2";
                varData.AddRange(prmOutput.Split(','));
                if (varData.Count < 5)
                {
                    throw new Exception("مقادیر ورودی نامعتبر می باشد.");
                }
                varLevel = "3";
                varSignData = SHA2(Keys.FngrKey, prmOutput.Substring(0, prmOutput.LastIndexOf(',')));
                varLevel = "4";
                if (!varSignData.Equals(varData[4]))
                {
                    throw new Exception("مقادیر ورودی نامعتبر می باشد.");
                }
                varLevel = "5";
                prmOutput = prmOutput.Substring(0, prmOutput.LastIndexOf(','));
                prmStrErr = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                prmStrErr = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return false;
            }
        }



        private class clsAES
        {
            private string prvAES_Key = string.Empty;
            private string prvAES_IV = string.Empty;

            public clsAES(string prmAES_Key, string prmAES_IV)
            {
                this.prvAES_Key = prmAES_Key;
                this.prvAES_IV = prmAES_IV;
            }

            public bool Encrypt(string prmInput, ref string prmOutput)
            {
                try
                {
                    using (Aes varAES = Aes.Create("AesManaged"))
                    {
                        varAES.BlockSize = 128;
                        varAES.KeySize = 256;
                        varAES.Padding = PaddingMode.PKCS7;
                        varAES.Key = Convert.FromBase64String(prvAES_Key);
                        varAES.IV = Convert.FromBase64String(prvAES_IV);
                        var varEncrpt = varAES.CreateEncryptor(varAES.Key, varAES.IV);
                        byte[] varBuffer = default(byte[]);
                        using (MemoryStream varMemStrm = new MemoryStream())
                        {
                            using (CryptoStream varCryptStrm = new CryptoStream(varMemStrm, varEncrpt, CryptoStreamMode.Write))
                            {
                                byte[] varXml = Encoding.UTF8.GetBytes(prmInput);
                                varCryptStrm.Write(varXml, 0, varXml.Length);
                            }
                            varBuffer = varMemStrm.ToArray();
                        }
                        prmOutput = Convert.ToBase64String(varBuffer);
                        return true;
                    }

                }
                catch 
                {
                    prmOutput = "";
                    return false;
                }
            }

            public bool Decrypt(string prmInput, ref string prmOutput)
            {
                try
                {
                    using (Aes varAES = Aes.Create("AesManaged"))
                    {
                        varAES.BlockSize = 128;
                        varAES.KeySize = 256;
                        varAES.Padding = PaddingMode.PKCS7;
                        varAES.Key = Convert.FromBase64String(prvAES_Key);
                        varAES.IV = Convert.FromBase64String(prvAES_IV);
                        var varDecrpt = varAES.CreateDecryptor();
                        byte[] varBuffer = default(byte[]);
                        using (System.IO.MemoryStream varMemStrm = new System.IO.MemoryStream())
                        {
                            using (CryptoStream varCryptStrm = new CryptoStream(varMemStrm, varDecrpt, CryptoStreamMode.Write))
                            {
                                byte[] varXml = Convert.FromBase64String(prmInput);
                                varCryptStrm.Write(varXml, 0, varXml.Length);
                            }
                            varBuffer = varMemStrm.ToArray();
                        }
                        prmOutput = Encoding.UTF8.GetString(varBuffer);
                        return true;
                    }

                }
                catch 
                {
                    prmOutput = "";
                    return false;
                }
            }

            public bool Generate(ref string prmKey, ref string prmIV)
            {
                try
                {
                    using (Aes varAES = Aes.Create("AesManaged"))
                    {
                        varAES.BlockSize = 128;
                        varAES.KeySize = 256;
                        varAES.Padding = PaddingMode.PKCS7;
                        varAES.GenerateKey();
                        varAES.GenerateIV();
                        prmKey = Convert.ToBase64String(varAES.Key);
                        prmIV = Convert.ToBase64String(varAES.IV);
                        return true;
                    }
                }
                catch
                {
                    prmKey = "";
                    prmIV = "";
                    return false;
                }
            }
        }
    }
}
