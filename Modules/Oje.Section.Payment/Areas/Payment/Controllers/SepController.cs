using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View.Sep;
using System.Threading.Tasks;

namespace Oje.Section.Payment.Areas.Payment.Controllers
{
    [Area("Payment")]
    [Route("[Area]/[Controller]/[Action]")]
    public class SepController : Controller
    {
        readonly IBankAccountFactorService BankAccountFactorService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IBankAccountDetectorService BankAccountDetectorService = null;
        readonly IBankAccountSepPaymentService BankAccountSepPaymentService = null;

        public SepController
            (
                IBankAccountFactorService BankAccountFactorService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IBankAccountDetectorService BankAccountDetectorService,
                IBankAccountSepPaymentService BankAccountSepPaymentService
            )
        {
            this.BankAccountFactorService = BankAccountFactorService;
            this.SiteSettingService = SiteSettingService;
            this.BankAccountDetectorService = BankAccountDetectorService;
            this.BankAccountSepPaymentService = BankAccountSepPaymentService;
        }

        [HttpGet]
        public async Task<ActionResult> Pay([FromQuery] string factorId)
        {
            var foundFactor = BankAccountFactorService.GetById(factorId, SiteSettingService.GetSiteSetting()?.Id);
            if (foundFactor == null || BankAccountDetectorService.GetByType(foundFactor.BankAccountId, SiteSettingService.GetSiteSetting()?.Id) != Infrastructure.Enums.BankAccountType.Sep)
                throw BException.GenerateNewException(BMessages.Not_Found);

            string generateToken = await BankAccountSepPaymentService.GenerateTokenAsync(factorId, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId);

            return Redirect(GlobalConfig.Configuration["PaymentUrls:SepPaymentG"] + "/OnlinePG/SendToken?token=" + generateToken);
        }

        [HttpPost]
        public async Task<ActionResult> Confirm([FromForm] SepCallBack input)
        {
            string redirectUrl = await BankAccountSepPaymentService.ConfirmPayment(input, SiteSettingService.GetSiteSetting()?.Id);
            if (string.IsNullOrEmpty(redirectUrl))
            {
                var foundFactor = BankAccountFactorService.GetByIdView(input.ResNum, SiteSettingService.GetSiteSetting()?.Id);
                if (foundFactor == null)
                    throw BException.GenerateNewException(BMessages.Payment_Was_UnsuccessFull);

                foundFactor.errorMessage = BMessages.Payment_Was_UnsuccessFull.GetEnumDisplayName();
                return View("postToPaymentPage", foundFactor);
            }

            return View("RedirectToPage", redirectUrl);
        }
    }
}
