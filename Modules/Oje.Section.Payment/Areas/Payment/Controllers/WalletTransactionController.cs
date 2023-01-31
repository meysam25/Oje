using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Models.View;

namespace Oje.Section.Payment.Areas.Payment.Controllers
{
    [Area("Payment")]
    [Route("[Area]/[Controller]/[Action]")]
    public class WalletTransactionController : Controller
    {
        readonly PaymentService.Interfaces.IWalletTransactionService WalletTransactionService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public WalletTransactionController
            (
                PaymentService.Interfaces.IWalletTransactionService WalletTransactionService,
                ISiteSettingService SiteSettingService
            )
        {
            this.WalletTransactionService = WalletTransactionService;
            this.SiteSettingService = SiteSettingService;
        }

        [HttpPost]
        public IActionResult Pay([FromForm] WalletTransactionPayVM input)
        {
            return Json(WalletTransactionService.Pay(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }
    }
}
