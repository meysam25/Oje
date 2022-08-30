using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View;
using Oje.PaymentService.Models.View.TiTec;
using Oje.PaymentService.Services.EContext;
using System.Net.Http.Headers;
using System.Text;

namespace Oje.PaymentService.Services
{
    public class TiTecService : ITiTecService
    {
        readonly ITitakUserService TitakUserService = null;
        readonly IHttpClientFactory HttpClientFactory = null;
        readonly IBankAccountFactorService BankAccountFactorService = null;
        readonly IUserService UserService = null;
        readonly IBankAccountService BankAccountService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        readonly PaymentDBContext db = null;

        static Dictionary<int, AccessTokenResult> accessTokens = new Dictionary<int, AccessTokenResult>();

        public TiTecService
            (
                ITitakUserService TitakUserService,
                IHttpClientFactory HttpClientFactory,
                IBankAccountFactorService BankAccountFactorService,
                IHttpContextAccessor HttpContextAccessor,
                IUserService UserService,
                IBankAccountService BankAccountService,
                PaymentDBContext db
            )
        {
            this.TitakUserService = TitakUserService;
            this.HttpClientFactory = HttpClientFactory;
            this.BankAccountFactorService = BankAccountFactorService;
            this.HttpContextAccessor = HttpContextAccessor;
            this.UserService = UserService;
            this.BankAccountService = BankAccountService;
            this.db = db;
        }

        public async Task<AccessTokenResult> LoginAsync(int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);

            if (accessTokens == null)
                accessTokens = new Dictionary<int, AccessTokenResult>();

            var accessToken = accessTokens.Keys.Any(t => t == siteSettingId) ? accessTokens[siteSettingId.ToIntReturnZiro()] : null;
            if (accessToken != null && !string.IsNullOrEmpty(accessToken.token) && accessToken.expiration != null && accessToken.expiration.Value > DateTime.Now.ToUniversalTime().AddMinutes(1))
                return accessToken;
            var userPassVM = TitakUserService.GetDbModelBy(siteSettingId);
            if (userPassVM == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Username);

            try
            {
                using (var stringContent = new StringContent(JsonConvert.SerializeObject(new { username = userPassVM.Username, password = userPassVM.Password.Decrypt() }, Formatting.Indented), Encoding.UTF8, "application/json"))
                using (var client = HttpClientFactory.CreateClient())
                {
                    client.BaseAddress = new Uri(GlobalConfig.Configuration["PaymentUrls:TitakPaymentG"] + ":9091");
                    HttpResponseMessage clientResult = await client.PostAsync("/api/Authenticate/login", stringContent);
                    var temp = await clientResult.Content.ReadAsStringAsync();
                    var accessResult = JsonConvert.DeserializeObject<AccessTokenResult>(temp);
                    if (clientResult.IsSuccessStatusCode)
                        if (accessResult != null && !string.IsNullOrEmpty(accessResult.token) && accessResult.expiration != null && accessResult.expiration.Value > DateTime.Now.ToUniversalTime())
                        {
                            accessTokens[siteSettingId.ToIntReturnZiro()] = accessResult;
                            return accessResult;
                        }
                        else
                            throw BException.GenerateNewException(BMessages.Invalid_User_Or_Password);
                    else
                        throw BException.GenerateNewException(BMessages.Invalid_User_Or_Password);
                }
            }
            catch
            {
                throw BException.GenerateNewException(BMessages.Invalid_User_Or_Password);
            }
        }

        public async Task<FactorResultVM> CreateFactorAsync(FactorVM input, int? siteSettingId)
        {
            createFactorValidation(input, siteSettingId);
            var loginToken = await LoginAsync(siteSettingId);
            if (loginToken == null)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);

            try
            {
                using (var stringContent = new StringContent(JsonConvert.SerializeObject(input, Formatting.Indented), Encoding.UTF8, "application/json"))
                using (var client = HttpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken.token);
                    client.BaseAddress = new Uri(GlobalConfig.Configuration["PaymentUrls:TitakPaymentG"] + ":6001");
                    HttpResponseMessage clientResult = await client.PostAsync("/api/Factor/Insert", stringContent);
                    var temp = await clientResult.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FactorResultVM>(temp);
                    if (clientResult.IsSuccessStatusCode)
                        if (result != null && result.status == 1 && !string.IsNullOrEmpty(result.factorNumber))
                            return result;
                        else if (result != null && result.status == 2)
                            throw BException.GenerateNewException(BMessages.Dublicate_Item);
                        else
                            throw BException.GenerateNewException(BMessages.UnknownError);
                    else
                        throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                }
            }
            catch (BException)
            {
                throw;
            }
            catch (Exception)
            {
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            }
        }



        private void createFactorValidation(FactorVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
        }

        public async Task<FactorLinkResultVM> CreateFactorLinkAsync(FactorLinkVM input, int? siteSettingId)
        {
            createFactorLinkValidation(input, siteSettingId);
            var loginToken = await LoginAsync(siteSettingId);
            if (loginToken == null)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);

            try
            {
                using (var stringContent = new StringContent(JsonConvert.SerializeObject(input, Formatting.Indented), Encoding.UTF8, "application/json"))
                using (var client = HttpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken.token);
                    client.BaseAddress = new Uri(GlobalConfig.Configuration["PaymentUrls:TitakPaymentG"] + ":6001");
                    HttpResponseMessage clientResult = await client.PostAsync("/api/Factor/InitPayment", stringContent);
                    var temp = await clientResult.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FactorLinkResultVM>(temp);
                    if (clientResult.IsSuccessStatusCode)
                        return result;
                    else
                        throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                }
            }
            catch (BException)
            {
                throw;
            }
            catch (Exception)
            {
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            }
        }

        private void createFactorLinkValidation(FactorLinkVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
        }

        public async Task<FactorPayInquiryResultVM> InquiryFactorAsync(FactorPayInquiryVM input, int? siteSettingId)
        {
            inquiryFactorValidation(input, siteSettingId);
            var loginToken = await LoginAsync(siteSettingId);
            if (loginToken == null)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);

            try
            {
                using (var stringContent = new StringContent(JsonConvert.SerializeObject(input, Formatting.Indented), Encoding.UTF8, "application/json"))
                using (var client = HttpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken.token);
                    client.BaseAddress = new Uri(GlobalConfig.Configuration["PaymentUrls:TitakPaymentG"] + ":6001");
                    HttpResponseMessage clientResult = await client.PostAsync("/api/Factor/PayInquiry", stringContent);
                    var temp = await clientResult.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FactorPayInquiryResultVM>(temp);
                    if (clientResult.IsSuccessStatusCode)
                        return result;
                    else
                        throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                }
            }
            catch (BException)
            {
                throw;
            }
            catch (Exception)
            {
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            }
        }

        public async Task<FactorPayInquiryResultVM> PaymentVerifyAsync(FactorPayInquiryVM input, int? siteSettingId)
        {
            inquiryFactorValidation(input, siteSettingId);
            var loginToken = await LoginAsync(siteSettingId);
            if (loginToken == null)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);

            try
            {
                using (var stringContent = new StringContent(JsonConvert.SerializeObject(input, Formatting.Indented), Encoding.UTF8, "application/json"))
                using (var client = HttpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken.token);
                    client.BaseAddress = new Uri(GlobalConfig.Configuration["PaymentUrls:TitakPaymentG"] + ":6001");
                    HttpResponseMessage clientResult = await client.PostAsync("/api/Factor/PaymentVerify", stringContent);
                    var temp = await clientResult.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FactorPayInquiryResultVM>(temp);
                    if (clientResult.IsSuccessStatusCode)
                        return result;
                    else
                        throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                }
            }
            catch (BException)
            {
                throw;
            }
            catch (Exception)
            {
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            }
        }

        private void inquiryFactorValidation(FactorPayInquiryVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
        }

        public async Task<string> PayAsync(string bankAccountFactorId, long? loginUserId, int? siteSettingId)
        {
            var foundFactor = BankAccountFactorService.GetById(bankAccountFactorId, siteSettingId);

            if (HttpContextAccessor == null)
                throw BException.GenerateNewException(BMessages.Somthing_Missing);
            if (foundFactor == null)
                throw BException.GenerateNewException(BMessages.Factor_Can_Not_Be_Founted);
            if (foundFactor.Price <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            var bankAccountUserInfo = BankAccountService.GetUserInfo(foundFactor.BankAccountId, siteSettingId);
            if (bankAccountUserInfo == null)
                throw BException.GenerateNewException(BMessages.User_Not_Found);

            var foundLoginUser = UserService.GetBy(loginUserId, siteSettingId);
            if (foundLoginUser == null)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);

            string factorId = foundFactor.BankAccountId + "_" + ((int)foundFactor.Type) + "_" + foundFactor.ObjectId + "_" + foundFactor.CreateDate.Ticks;
            await CreateFactorAsync(new FactorVM()
            {
                CallbackUrl = HttpContextAccessor.HttpContext.Request.Scheme + "://" + HttpContextAccessor.HttpContext.Request.Host + "/Payment/TITec/Confirm",
                Amount = foundFactor.Price,
                AmountPayable = foundFactor.Price,
                CustomerMobileNo = foundLoginUser.Username,
                FactorDate = foundFactor.CreateDate.ToFaDate(),
                CustomerName = foundLoginUser.Firstname + " " + foundLoginUser.Lastname,
                Description = "پرداخت بابت فرم پیشنهاد",
                FactorNumber = factorId,
                SendFactorToCustomer = false,
                WalletCharge = 0,
                Shares = new List<FactorShare>()
                {
                    new FactorShare()
                    {
                        iban = bankAccountUserInfo.shabaNO,
                        MobileNo = bankAccountUserInfo.mobileNo,
                        ShareHolderName = bankAccountUserInfo.fullname,
                        SharePercent = 100
                    }
                }
            }, siteSettingId);
            var paymentLink = await CreateFactorLinkAsync(new FactorLinkVM() { FactorNumber = factorId, InitType = FactorLinkInitType.LinkText }, siteSettingId);

            if (paymentLink == null || string.IsNullOrEmpty(paymentLink.url))
                throw BException.GenerateNewException(BMessages.Payment_Was_UnsuccessFull);

            return paymentLink.url;
        }

        public async Task InquiryFactorForWorkerAsync()
        {
            var curDT = DateTime.Now.AddHours(-1);
            var allItems = db.BankAccountFactors.Where(t => t.LastTryDate == null && t.IsPayed == false && (t.CountTry == null || t.CountTry == 0) && !string.IsNullOrEmpty(t.TraceCode) && t.BankAccountType == BankAccountType.titec).ToList();
            if (allItems.Count == 0)
                allItems = db.BankAccountFactors.Where(t => t.LastTryDate != null && t.IsPayed == false && curDT > t.LastTryDate && t.CountTry <= 73 && !string.IsNullOrEmpty(t.TraceCode) && t.BankAccountType == BankAccountType.titec).ToList();
            foreach (var item in allItems)
            {
                item.LastTryDate = DateTime.Now;
                if (item.CountTry == null)
                    item.CountTry = 0;
            }

            db.SaveChanges();

            foreach (var item in allItems)
            {
                FactorPayInquiryResultVM resultInquiry = null;
                try
                {
                    resultInquiry = await InquiryFactorAsync(new FactorPayInquiryVM() { factorNumber = item.BankAccountId + "-" + ((int)item.Type) + "-" + item.ObjectId + "-" + item.CreateDate.Ticks }, item.SiteSettingId);
                }
                catch (Exception ex)
                {
                    item.CountTry++;
                    item.LastErrorMessage = ex.Message;
                    continue;
                };
                if (resultInquiry != null && resultInquiry.status == true && resultInquiry.payDateTime != null)
                {
                    BankAccountFactorService.UpdatePaymentInfor(item, item.TraceCode, item.SiteSettingId, resultInquiry.payDateTime);
                }
                else if (resultInquiry != null)
                    item.CountTry++;
                else
                {
                    item.CountTry++;
                    item.LastErrorMessage = BMessages.UnknownError.GetEnumDisplayName();
                }
            }

            db.SaveChanges();
        }

        private bool validateConfirmPaymentInput(TiTecConfirmPaymentInput input)
        {
            return
                input != null && !string.IsNullOrEmpty(input.FactorId) &&
                !string.IsNullOrEmpty(input.FactorNo) && input.Amount > 0;
        }

        public async Task<string> ConfirmPayment(TiTecConfirmPaymentInput input, int? siteSettingId)
        {
            string result = "";

            if (validateConfirmPaymentInput(input))
            {
                var foundFactor = BankAccountFactorService.GetById(input.FactorNo, siteSettingId);
                if (foundFactor != null && foundFactor.IsPayed != true && foundFactor.Price > 0 && foundFactor.PayDate == null)
                {
                    var eResult = await InquiryFactorAsync(new FactorPayInquiryVM() { factorNumber = input.FactorNo }, siteSettingId);
                    if (eResult != null && eResult.status == true && input.Amount == input.Amount && input.FactorNo == eResult.factorNumber && eResult.payDate != null && input.FactorId == eResult.factorId)
                    {
                        BankAccountFactorService.UpdatePaymentInfor(foundFactor, eResult.factorId, siteSettingId);
                        result = foundFactor.TargetLink;
                    }
                }
            }

            return result;
        }
    }
}
