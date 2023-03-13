using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View.Sadad;
using System.Security.Cryptography;
using System.Text;

namespace Oje.PaymentService.Services
{
    public class BankAccountSadadPaymentService : IBankAccountSadadPaymentService
    {
        readonly IBankAccountSadadService BankAccountSadadService = null;
        readonly IBankAccountFactorService BankAccountFactorService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        readonly IHttpClientFactory HttpClientFactory = null;
        readonly IUserService UserService = null;

        static object lockObj = null;

        public BankAccountSadadPaymentService
            (
                IBankAccountSadadService BankAccountSadadService,
                IBankAccountFactorService BankAccountFactorService,
                IHttpContextAccessor HttpContextAccessor,
                IHttpClientFactory HttpClientFactory,
                IUserService UserService
            )
        {
            this.BankAccountSadadService = BankAccountSadadService;
            this.BankAccountFactorService = BankAccountFactorService;
            this.HttpContextAccessor = HttpContextAccessor;
            this.HttpClientFactory = HttpClientFactory;
            this.UserService = UserService;
        }

        string signData(byte[] input, string MerchantKey)
        {
            using (var symmetric = SymmetricAlgorithm.Create("TripleDes"))
            {
                symmetric.Mode = CipherMode.ECB;
                symmetric.Padding = PaddingMode.PKCS7;
                using (var encryptor = symmetric.CreateEncryptor(Convert.FromBase64String(MerchantKey), new byte[8]))
                {
                    return Convert.ToBase64String(encryptor.TransformFinalBlock(input, 0, input.Length));
                }
            }
        }

        public async Task<string> PaymentRequestAsync(string bankAccountFactorId, int? siteSettingId, long? loginUserId)
        {
            var foundFactor = BankAccountFactorService.GetById(bankAccountFactorId, siteSettingId);
            if (HttpContextAccessor == null)
                throw BException.GenerateNewException(BMessages.Somthing_Missing);
            if (foundFactor == null)
                throw BException.GenerateNewException(BMessages.Factor_Can_Not_Be_Founted);
            if (foundFactor.Price <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            var foundAccount = BankAccountSadadService.GetBy(foundFactor.BankAccountId, siteSettingId);
            if (foundAccount == null)
                throw BException.GenerateNewException(BMessages.Account_Can_Not_Be_Founded);
            if (!foundAccount.IsSignature())
                throw BException.GenerateNewException(BMessages.UnknownError);
            var foundLoginUser = UserService.GetBy(loginUserId, siteSettingId);

            var tempFactorId = foundFactor.KeyHash;

            var dataBytes = Encoding.UTF8.GetBytes(string.Format("{0};{1};{2}", foundAccount.TerminalId, tempFactorId, foundFactor.Price));

            using (var stringContent = new StringContent(JsonConvert.SerializeObject(new
            {
                OrderId = tempFactorId,
                SignData = signData(dataBytes, foundAccount.TerminalKey),
                ReturnUrl = HttpContextAccessor.HttpContext.Request.Scheme + "://" + HttpContextAccessor.HttpContext.Request.Host + "/Payment/Sadad/Confirm",
                TerminalId = foundAccount.TerminalId,
                MerchantId = foundAccount.MerchantId,
                Amount = foundFactor.Price,
                LocalDateTime = DateTime.Now,
                UserId = foundLoginUser.Username
            }, Formatting.Indented), Encoding.UTF8, "application/json"))
            using (var client = HttpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(GlobalConfig.Configuration["PaymentUrls:SadadPaymentG"]);
                HttpResponseMessage clientResult = await client.PostAsync("/api/v0/Request/PaymentRequest", stringContent);
                var varTokenResp = JsonConvert.DeserializeObject<PayResultData>(await clientResult.Content.ReadAsStringAsync());
                if (varTokenResp != null && varTokenResp.ResCode == "0" && clientResult.IsSuccessStatusCode && !string.IsNullOrEmpty(varTokenResp?.Token))
                {
                    foundFactor.Token = varTokenResp?.Token;
                    BankAccountFactorService.Save();
                    return varTokenResp?.Token;
                }
                else
                    throw BException.GenerateNewException(varTokenResp?.ResCode + " - خطا در اتصال به سرور درگاه پرداخت - " + varTokenResp.Description);
            }
        }

        public async Task<string> ConfirmPayment(PurchaseResult input, int? siteSettingId)
        {
            string result = "";

            if (validateConfirmPaymentInput(input))
            {
                var foundFactor = BankAccountFactorService.GetBy(input.OrderId, siteSettingId);
                if (foundFactor != null && foundFactor.IsPayed != true && foundFactor.Price > 0 && foundFactor.PayDate == null && !string.IsNullOrEmpty(foundFactor.Token))
                {
                    var foundAccount = BankAccountSadadService.GetBy(foundFactor.BankAccountId, siteSettingId);
                    if (foundAccount != null)
                    {
                        var dataBytes = Encoding.UTF8.GetBytes(foundFactor.Token);
                        var symmetric = SymmetricAlgorithm.Create("TripleDes");
                        symmetric.Mode = CipherMode.ECB;
                        symmetric.Padding = PaddingMode.PKCS7;

                        var encryptor = symmetric.CreateEncryptor(Convert.FromBase64String(foundAccount.TerminalKey), new byte[8]);
                        var signedData = Convert.ToBase64String(encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length));


                        using (var stringContent = new StringContent(JsonConvert.SerializeObject(new
                        {
                            token = foundFactor.Token,
                            SignData = signedData
                        }, Formatting.Indented), Encoding.UTF8, "application/json"))
                        using (var client = HttpClientFactory.CreateClient())
                        {
                            client.BaseAddress = new Uri(GlobalConfig.Configuration["PaymentUrls:SadadPaymentG"]);
                            HttpResponseMessage clientResult = await client.PostAsync("/api/v0/Advice/Verify", stringContent);
                            var temp = await clientResult.Content.ReadAsStringAsync();
                            var confirmResult = JsonConvert.DeserializeObject<VerifyResultData>(temp);
                            if (clientResult.IsSuccessStatusCode)
                            {
                                if (((confirmResult.ResCode == "0") || (confirmResult.ResCode == "100")) && confirmResult.OrderId == input.OrderId && confirmResult.Amount.ToLongReturnZiro() > 0 && confirmResult.Amount.ToLongReturnZiro() == foundFactor.Price && !string.IsNullOrEmpty(confirmResult.RetrivalRefNo))
                                {
                                    if (lockObj == null)
                                        lockObj = new object();
                                    lock (lockObj)
                                    {
                                        if (!BankAccountFactorService.ExistBy(confirmResult.RetrivalRefNo))
                                        {
                                            BankAccountFactorService.UpdatePaymentInfor(foundFactor, confirmResult.RetrivalRefNo, siteSettingId);
                                            result = foundFactor.TargetLink;
                                        }
                                    }
                                }
                                else
                                    throw BException.GenerateNewException(confirmResult.Description);
                            }
                            else
                                throw BException.GenerateNewException(confirmResult.ResCode + " - خطا در اتصال به سرور درگاه پرداخت - " + confirmResult.Description);
                        }
                    }
                }
            }

            return result;
        }

        private bool validateConfirmPaymentInput(PurchaseResult input)
        {
            return (input != null && !string.IsNullOrEmpty(input.OrderId) && !string.IsNullOrEmpty(input.token) && input.ResCode == "0");
        }
    }
}
