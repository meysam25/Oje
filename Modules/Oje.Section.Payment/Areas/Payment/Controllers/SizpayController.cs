using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View.SizPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Payment.Areas.Payment.Controllers
{
    [Area("Payment")]
    [Route("[Area]/[Controller]/[Action]")]
    public class SizpayController : Controller
    {
        readonly IBankAccountFactorService BankAccountFactorService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IBankAccountDetectorService BankAccountDetectorService = null;
        readonly IBankAccountSizpayPaymentService BankAccountSizpayPaymentService = null;
        public SizpayController
            (
                IBankAccountFactorService BankAccountFactorService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IBankAccountDetectorService BankAccountDetectorService,
                IBankAccountSizpayPaymentService BankAccountSizpayPaymentService
            )
        {
            this.BankAccountFactorService = BankAccountFactorService;
            this.SiteSettingService = SiteSettingService;
            this.BankAccountDetectorService = BankAccountDetectorService;
            this.BankAccountSizpayPaymentService = BankAccountSizpayPaymentService;
        }

        public async Task<ActionResult> Pay([FromQuery] string factorId)
        {
            var foundFactor = BankAccountFactorService.GetById(factorId, SiteSettingService.GetSiteSetting()?.Id);
            if (foundFactor == null || BankAccountDetectorService.GetByType(foundFactor.BankAccountId, SiteSettingService.GetSiteSetting()?.Id) != Infrastructure.Enums.BankAccountType.sizpay)
                return NotFound();

            var generateToken = await BankAccountSizpayPaymentService.GenerateToken(factorId, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId);

            return View(generateToken);
        }

        public async Task<ActionResult> Confirm([FromForm] SizpayConfirmPaymentInput input)
        {
            string redirectUrl = await BankAccountSizpayPaymentService.ConfirmPayment(input, SiteSettingService.GetSiteSetting()?.Id);
            if (string.IsNullOrEmpty(redirectUrl))
                throw BException.GenerateNewException(BMessages.Payment_Was_UnsuccessFull);

            return Redirect(redirectUrl);
        }
    }
}
