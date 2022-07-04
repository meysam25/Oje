using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using System;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "لاگ فعالیت وب سایت")]
    [CustomeAuthorizeFilter]
    public class BlockAutoIpController: Controller
    {
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IErrorService ErrorService = null;

        public BlockAutoIpController
            (
                IBlockAutoIpService BlockAutoIpService,
                ISiteSettingService SiteSettingService,
                IErrorService ErrorService
            )
        {
            this.BlockAutoIpService = BlockAutoIpService;
            this.SiteSettingService = SiteSettingService;
            this.ErrorService = ErrorService;
        }

        [AreaConfig(Title = "لاگ فعالیت وب سایت", Icon = "fa-user-secret", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لاگ فعالیت وب سایت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "BlockAutoIp", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست لاگ فعالیت وب سایت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "BlockAutoIp")));
        }

        [AreaConfig(Title = "مشاهده جزئیات خطای لاگ", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] string id)
        {
            return Json(ErrorService.GetBy(id));
        }

        [AreaConfig(Title = "مشاهده لیست لاگ فعالیت وب سایت", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] BlockAutoIpMainGrid searchInput)
        {
            return Json(BlockAutoIpService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BlockAutoIpMainGrid searchInput)
        {
            var result = BlockAutoIpService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
