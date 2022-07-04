using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.FinancialBaseData.Areas.FinancialBaseData.Controllers
{
    [Area("FinancialBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "اطلاعات پایه مالی", Icon = "fa-dollar-sign", Title = "تراکنش های کیف پول من")]
    [CustomeAuthorizeFilter]
    public class WalletTransactionController: Controller
    {
        readonly IWalletTransactionService WalletTransactionService = null;
        readonly IUserService UserService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;

        public WalletTransactionController
            (
                IWalletTransactionService WalletTransactionService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IUserService UserService
            )
        {
            this.WalletTransactionService = WalletTransactionService;
            this.SiteSettingService = SiteSettingService;
            this.UserService = UserService;
        }

        [AreaConfig(Title = "تراکنش های کیف پول من", Icon = "fa-wallet", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تراکنش های کیف پول من";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "WalletTransaction", new { area = "FinancialBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه تراکنش های کیف پول من", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FinancialBaseData", "WalletTransaction")));
        }

        [AreaConfig(Title = "افزایش موجودی کیف پول", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] WalletTransactionCreateUpdateVM input)
        {
            return Json(
                    WalletTransactionService.GeneratePaymentUrl(
                        input, 
                        SiteSettingService.GetSiteSetting()?.Id, 
                        HttpContext.GetLoginUser()?.UserId, 
                        Url.Action("Index", "WalletTransaction", new { area = "FinancialBaseData" }), 
                        UserService.GetMainPaymentUserId(SiteSettingService.GetSiteSetting()?.Id)
                    )
                );
        }

        [AreaConfig(Title = "مشاهده موجودی حساب", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetTitle()
        {
            return Content(WalletTransactionService.GetUserBlance(SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست تراکنش های کیف پول من", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] WalletTransactionMainGrid searchInput)
        {
            return Json(WalletTransactionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] WalletTransactionMainGrid searchInput)
        {
            var result = WalletTransactionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
