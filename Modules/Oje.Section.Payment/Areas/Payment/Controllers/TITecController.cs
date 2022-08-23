using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View;
using System.Threading.Tasks;

namespace Oje.Section.Payment.Areas.Payment.Controllers
{
    [Area("Payment")]
    [Route("[Area]/[Controller]/[Action]")]
    public class TITecController : Controller
    {
        readonly IBankAccountFactorService BankAccountFactorService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IBankAccountDetectorService BankAccountDetectorService = null;
        readonly ITiTecService TiTecService = null;

        public TITecController
           (
               IBankAccountFactorService BankAccountFactorService,
               AccountService.Interfaces.ISiteSettingService SiteSettingService,
               IBankAccountDetectorService BankAccountDetectorService,
               ITiTecService TiTecService
           )
        {
            this.BankAccountFactorService = BankAccountFactorService;
            this.SiteSettingService = SiteSettingService;
            this.BankAccountDetectorService = BankAccountDetectorService;
            this.TiTecService = TiTecService;
        }

        [HttpGet]
        public async Task<ActionResult> Pay([FromQuery] string factorId)
        {
            var foundFactor = BankAccountFactorService.GetById(factorId, SiteSettingService.GetSiteSetting()?.Id);
            if (foundFactor == null || BankAccountDetectorService.GetByType(foundFactor.BankAccountId, SiteSettingService.GetSiteSetting()?.Id) != Infrastructure.Enums.BankAccountType.titec)
                throw BException.GenerateNewException(BMessages.Not_Found);

            string payUrl = await TiTecService.PayAsync(factorId, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id);

            return View("Pay", payUrl);
        }

        [HttpPost]
        public async Task<ActionResult> Confirm([FromForm] TiTecConfirmPaymentInput input)
        {
            string redirectUrl = await TiTecService.ConfirmPayment(input, SiteSettingService.GetSiteSetting()?.Id);
            if (string.IsNullOrEmpty(redirectUrl))
                throw BException.GenerateNewException(BMessages.Payment_Was_UnsuccessFull);

            return Redirect(redirectUrl);
        }
    }
}
