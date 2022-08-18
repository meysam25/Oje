using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View.Sep;
using System.Text;

namespace Oje.PaymentService.Services
{
    public class BankAccountSepPaymentService : IBankAccountSepPaymentService
    {
        readonly IBankAccountFactorService BankAccountFactorService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        readonly IBankAccountSepService BankAccountSepService = null;
        readonly IUserService UserService = null;
        readonly IHttpClientFactory HttpClientFactory = null;

        static object lockObj = null;

        public BankAccountSepPaymentService
            (
                IBankAccountFactorService BankAccountFactorService,
                IHttpContextAccessor HttpContextAccessor,
                IBankAccountSepService BankAccountSepService,
                IUserService UserService,
                IHttpClientFactory HttpClientFactory
            )
        {
            this.BankAccountFactorService = BankAccountFactorService;
            this.HttpContextAccessor = HttpContextAccessor;
            this.BankAccountSepService = BankAccountSepService;
            this.UserService = UserService;
            this.HttpClientFactory = HttpClientFactory;
        }

        public async Task<string> ConfirmPayment(SepCallBack input, int? siteSettingId)
        {
            string result = "";

            if (validateConfirmPaymentInput(input))
            {
                var foundFactor = BankAccountFactorService.GetById(input.ResNum, siteSettingId);
                if (foundFactor != null && foundFactor.IsPayed != true && foundFactor.Price > 0 && foundFactor.PayDate == null)
                {
                    var foundAccount = BankAccountSepService.GetBy(foundFactor.BankAccountId, siteSettingId);
                    if (foundAccount != null)
                    {
                        using (var stringContent = new StringContent(JsonConvert.SerializeObject(new
                        {
                            RefNum = input.RefNum,
                            TerminalNumber = foundAccount.TerminalId
                        }, Formatting.Indented), Encoding.UTF8, "application/json"))
                        using (var client = HttpClientFactory.CreateClient())
                        {
                            client.BaseAddress = new Uri(GlobalConfig.Configuration["PaymentUrls:SepPaymentG"]);
                            HttpResponseMessage clientResult = await client.PostAsync("/verifyTxnRandomSessionkey/ipg/VerifyTransaction", stringContent);
                            var temp = await clientResult.Content.ReadAsStringAsync();
                            var confirmResult = JsonConvert.DeserializeObject<VerifyResultData>(temp);
                            if (clientResult.IsSuccessStatusCode)
                            {
                                if (
                                     ((confirmResult.ResultCode == 0 && confirmResult.Success == true) || (confirmResult.ResultCode == 2 && confirmResult.Success == false))
                                     && confirmResult.TransactionDetail != null  && confirmResult.TransactionDetail.OrginalAmount > 0 && confirmResult.TransactionDetail.OrginalAmount == foundFactor.Price && input.TraceNo == confirmResult.TransactionDetail.StraceNo)
                                {
                                    if (lockObj == null)
                                        lockObj = new object();
                                    lock (lockObj)
                                    {
                                        if (!BankAccountFactorService.ExistBy(input.TraceNo))
                                        {
                                            BankAccountFactorService.UpdatePaymentInfor(foundFactor, input.TraceNo, siteSettingId);
                                            result = foundFactor.TargetLink;
                                        }
                                    }
                                }
                                else
                                    throw BException.GenerateNewException(confirmResult.ResultDescription);
                            }
                            else
                                throw BException.GenerateNewException(confirmResult.ResultCode + " - خطا در اتصال به سرور درگاه پرداخت - " + confirmResult.ResultDescription);
                        }
                    }
                }
            }

            return result;
        }

        private bool validateConfirmPaymentInput(SepCallBack input)
        {
            return input != null && input.Status == 2 && !string.IsNullOrEmpty(input.RefNum) && !string.IsNullOrEmpty(input.TraceNo) && input.Amount > 0 && !string.IsNullOrEmpty(input.ResNum);
        }

        public async Task<string> GenerateTokenAsync(string factorId, int? siteSettingId, long? loginUserId)
        {
            var foundFactor = BankAccountFactorService.GetById(factorId, siteSettingId);
            if (HttpContextAccessor == null)
                throw BException.GenerateNewException(BMessages.Somthing_Missing);
            if (foundFactor == null)
                throw BException.GenerateNewException(BMessages.Factor_Can_Not_Be_Founted);
            if (foundFactor.Price <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            var foundAccount = BankAccountSepService.GetBy(foundFactor.BankAccountId, siteSettingId);
            if (foundAccount == null)
                throw BException.GenerateNewException(BMessages.Account_Can_Not_Be_Founded);
            var foundLoginUser = UserService.GetBy(loginUserId, siteSettingId);

            using (var stringContent = new StringContent(JsonConvert.SerializeObject(new
            {
                action = "token",
                ResNum = factorId,
                RedirectUrl = HttpContextAccessor.HttpContext.Request.Scheme + "://" + HttpContextAccessor.HttpContext.Request.Host + "/Payment/Sep/Confirm",
                TerminalId = foundAccount.TerminalId,
                Amount = foundFactor.Price,
                CellNumber = !string.IsNullOrEmpty(foundLoginUser.Username) && foundLoginUser.Username.StartsWith("0") ? foundLoginUser.Username.Substring(0, foundLoginUser.Username.Length - 1) : foundLoginUser.Username,
            }, Formatting.Indented), Encoding.UTF8, "application/json"))
            using (var client = HttpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(GlobalConfig.Configuration["PaymentUrls:SepPaymentG"]);
                HttpResponseMessage clientResult = await client.PostAsync("/onlinepg/onlinepg", stringContent);
                var varTokenResp = JsonConvert.DeserializeObject<TokenResult>(await clientResult.Content.ReadAsStringAsync());
                if (varTokenResp != null && varTokenResp.status == 1 && clientResult.IsSuccessStatusCode && !string.IsNullOrEmpty(varTokenResp?.token))
                {
                    foundFactor.Token = varTokenResp?.token;
                    BankAccountFactorService.Save();
                    return varTokenResp?.token;
                }
                else
                    throw BException.GenerateNewException(varTokenResp?.status + " - خطا در اتصال به سرور درگاه پرداخت - " + varTokenResp.errorDesc);
            }
        }
    }
}
