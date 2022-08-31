using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View.Sadad;
using System.Threading.Tasks;

namespace Oje.Section.Payment.Areas.Payment.Controllers
{
    [Area("Payment")]
    [Route("[Area]/[Controller]/[Action]")]
    public class SadadController : Controller
    {
        readonly IBankAccountFactorService BankAccountFactorService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IBankAccountDetectorService BankAccountDetectorService = null;
        readonly IBankAccountSadadPaymentService BankAccountSadadPaymentService = null;

        public SadadController
            (
                IBankAccountFactorService BankAccountFactorService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IBankAccountDetectorService BankAccountDetectorService,
                IBankAccountSadadPaymentService BankAccountSadadPaymentService
            )
        {
            this.BankAccountFactorService = BankAccountFactorService;
            this.SiteSettingService = SiteSettingService;
            this.BankAccountDetectorService = BankAccountDetectorService;
            this.BankAccountSadadPaymentService = BankAccountSadadPaymentService;
        }

        [HttpGet]
        public async Task<ActionResult> Pay([FromQuery] string factorId)
        {
            var foundFactor = BankAccountFactorService.GetById(factorId, SiteSettingService.GetSiteSetting()?.Id);
            if (foundFactor == null || BankAccountDetectorService.GetByType(foundFactor.BankAccountId, SiteSettingService.GetSiteSetting()?.Id) != Infrastructure.Enums.BankAccountType.sadad)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var generateToken = await BankAccountSadadPaymentService.PaymentRequestAsync(factorId, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId);

            return Redirect(GlobalConfig.Configuration["PaymentUrls:SadadPaymentG"] + "/Purchase/Index?token=" +  generateToken);
        }

        [HttpPost]
        public async Task<ActionResult> Confirm([FromForm] PurchaseResult input)
        {
            string redirectUrl = await BankAccountSadadPaymentService.ConfirmPayment(input, SiteSettingService.GetSiteSetting()?.Id);
            if (string.IsNullOrEmpty(redirectUrl))
            {
                var foundFactor = BankAccountFactorService.GetByIdView(input.OrderId.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id);
                if (foundFactor == null)
                    throw BException.GenerateNewException(BMessages.Payment_Was_UnsuccessFull);

                foundFactor.errorMessage = BMessages.Payment_Was_UnsuccessFull.GetEnumDisplayName();
                return View("postToPaymentPage", foundFactor);
            }

            return View("RedirectToPage", redirectUrl);
        }
    }
}
