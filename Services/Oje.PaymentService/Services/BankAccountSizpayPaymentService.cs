using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View;
using Oje.PaymentService.Models.View.SizPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Services
{
    public class BankAccountSizpayPaymentService : IBankAccountSizpayPaymentService
    {
        readonly IBankAccountSizpayService BankAccountSizpayService = null;
        readonly IBankAccountFactorService BankAccountFactorService = null;
        readonly IHttpContextAccessor HttpContextAccessor;
        readonly ISizpayCryptoService SizpayCryptoService = null;
        readonly IUserService UserService = null;
        readonly IHttpClientFactory HttpClientFactory = null;
        public BankAccountSizpayPaymentService(
                IBankAccountSizpayService BankAccountSizpayService,
                IBankAccountFactorService BankAccountFactorService,
                IHttpContextAccessor HttpContextAccessor,
                ISizpayCryptoService SizpayCryptoService,
                IUserService UserService,
                IHttpClientFactory HttpClientFactory
            )
        {
            this.BankAccountSizpayService = BankAccountSizpayService;
            this.BankAccountFactorService = BankAccountFactorService;
            this.HttpContextAccessor = HttpContextAccessor;
            this.SizpayCryptoService = SizpayCryptoService;
            this.UserService = UserService;
            this.HttpClientFactory = HttpClientFactory;
        }

        public async Task<string> ConfirmPayment(SizpayConfirmPaymentInput input, int? siteSettingId)
        {
            string result = "";

            if (validateConfirmPaymentInput(input))
            {
                var foundFactor = BankAccountFactorService.GetById(input.InvoiceNo, siteSettingId);
                if (foundFactor != null && foundFactor.IsPayed != true && foundFactor.Price > 0 && foundFactor.PayDate == null)
                {
                    var foundAccount = BankAccountSizpayService.GetBy(foundFactor.BankAccountId, siteSettingId);
                    if (foundAccount != null)
                    {
                        var eResult = SizpayCryptoService
                             .Encrypt(
                                     string.Join(",", foundAccount.MerchantId, foundAccount.TerminalId, input.Token),
                                     new CryptoModels(foundAccount.FirstKey, foundAccount.SecondKey, foundAccount.SignKey)
                                 );
                        if (eResult != null && eResult.isSuccess && !string.IsNullOrEmpty(eResult.result))
                        {
                            using (var stringContent = new StringContent(JsonConvert.SerializeObject(new SizPayConfirmPaymentPost() { MerchantID = foundAccount.MerchantId + "", TerminalID = foundAccount.TerminalId + "", Token = input.Token, SignData = eResult.result }, Formatting.Indented), Encoding.UTF8, "application/json"))
                            using (var client = HttpClientFactory.CreateClient())
                            {
                                client.BaseAddress = new Uri(GlobalConfig.Configuration["PaymentUrls:SizpayPaymentG"]);
                                HttpResponseMessage clientResult = await client.PostAsync("/api/Payment/Confirm", stringContent);
                                var temp = await clientResult.Content.ReadAsStringAsync();
                                var confirmResult = JsonConvert.DeserializeObject<SizPayConfirmPaymentPostResult>(temp);
                                if (clientResult.IsSuccessStatusCode)
                                {
                                    if (confirmResult.ResCod == 0)
                                    {
                                        if(confirmResult.Amount == foundFactor.Price && !string.IsNullOrEmpty(confirmResult.TraceNo) && confirmResult.InvoiceNo == input.InvoiceNo)
                                        {
                                            if(!BankAccountFactorService.ExistBy(confirmResult.TraceNo))
                                            {
                                                BankAccountFactorService.UpdatePaymentInfor(foundFactor, confirmResult.TraceNo);
                                                result = foundFactor.TargetLink;
                                            }
                                        }
                                    }
                                    else
                                        throw BException.GenerateNewException(confirmResult.Message);
                                }
                                else
                                    throw BException.GenerateNewException(confirmResult.ResCod + " - خطا در اتصال به سرور درگاه پرداخت - " + confirmResult.Message);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool validateConfirmPaymentInput(SizpayConfirmPaymentInput input)
        {
            return
                input != null && input.ResCod == 0 && !string.IsNullOrEmpty(input.OrderID) &&
                !string.IsNullOrEmpty(input.InvoiceNo) && !string.IsNullOrEmpty(input.Token);
        }

        public async Task<GenerateTokenMethodResultVM> GenerateToken(string bankAccountFactorId, int? siteSettingId, long? loginUserId)
        {
            string result = "";

            var foundFactor = BankAccountFactorService.GetById(bankAccountFactorId, siteSettingId);

            if (HttpContextAccessor == null)
                throw BException.GenerateNewException(BMessages.Somthing_Missing);
            if (foundFactor == null)
                throw BException.GenerateNewException(BMessages.Factor_Can_Not_Be_Founted);
            if (foundFactor.Price <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            var foundAccount = BankAccountSizpayService.GetBy(foundFactor.BankAccountId, siteSettingId);
            if (foundAccount == null)
                throw BException.GenerateNewException(BMessages.Account_Can_Not_Be_Founded);

            var foundLoginUser = UserService.GetBy(loginUserId, siteSettingId);
            SizpayToken sizpayToken = new SizpayToken();
            sizpayToken.ReturnURL = HttpContextAccessor.HttpContext.Request.Scheme + "://" + HttpContextAccessor.HttpContext.Request.Host + "/Payment/Sizpay/Confirm";
            sizpayToken.AppExtraInf = JsonConvert.SerializeObject(new
            {
                Descr = "",
                PayerEmail = foundLoginUser?.Email?.Trim(),
                PayerMobile = foundLoginUser?.Mobile?.Trim(),
                PayerNm = foundLoginUser?.Firstname?.Trim() + " " + foundLoginUser.Lastname?.Trim(),
                PayerIP = HttpContextAccessor.GetIpAddress() + ""
            });
            sizpayToken.OrderID = Guid.NewGuid() + "";
            sizpayToken.InvoiceNo = bankAccountFactorId;
            sizpayToken.Amount = foundFactor.Price;
            sizpayToken.MerchantID = foundAccount.MerchantId + "";
            sizpayToken.TerminalID = foundAccount.TerminalId + "";

            var eResult = SizpayCryptoService
                .Encrypt(
                        string.Join(",", foundAccount.MerchantId, foundAccount.TerminalId, foundFactor.Price.ToString(), "", sizpayToken.OrderID, sizpayToken.ReturnURL, sizpayToken.ExtraInf, sizpayToken.InvoiceNo),
                        new CryptoModels(foundAccount.FirstKey, foundAccount.SecondKey, foundAccount.SignKey)
                    );
            if (eResult.isSuccess == false)
                throw BException.GenerateNewException(eResult.message);
            if (string.IsNullOrEmpty(eResult.result))
                throw BException.GenerateNewException(BMessages.Validation_Error);

            sizpayToken.SignData = eResult.result;


            using (var stringContent = new StringContent(JsonConvert.SerializeObject(sizpayToken, Formatting.Indented), Encoding.UTF8, "application/json"))
            using (var client = HttpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(GlobalConfig.Configuration["PaymentUrls:SizpayPaymentG"]);
                HttpResponseMessage clientResult = await client.PostAsync("/api/Payment/GetToken", stringContent);
                var varTokenResp = JsonConvert.DeserializeObject<SizPayTokenResponse>(await clientResult.Content.ReadAsStringAsync());
                if (clientResult.IsSuccessStatusCode)
                {
                    if (varTokenResp.ResCod == 0)
                        result = varTokenResp.Token;
                    else
                        throw BException.GenerateNewException(varTokenResp.Message);
                }
                else
                    throw BException.GenerateNewException(varTokenResp.ResCod + " - خطا در اتصال به سرور درگاه پرداخت - " + varTokenResp.Message);
            }


            return new GenerateTokenMethodResultVM() { token = result, merchantID = foundAccount.MerchantId, terminalID = foundAccount.TerminalId };
        }
    }
}
