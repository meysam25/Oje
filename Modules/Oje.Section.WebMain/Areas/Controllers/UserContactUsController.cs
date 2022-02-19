using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Areas.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "تماس با ما کاربران")]
    [CustomeAuthorizeFilter]
    public class UserContactUsController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IContactUsService ContactUsService = null;
        public UserContactUsController
            (
                IContactUsService ContactUsService, 
                ISiteSettingService SiteSettingService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.ContactUsService = ContactUsService;
        }

        [AreaConfig(Title = "تماس با ما کاربران", Icon = "fa-info", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تماس با ما کاربران";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserContactUs", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تماس با ما کاربران", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "UserContactUs")));
        }

        [AreaConfig(Title = "مشاهده تماس با ما کاربران", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetById([FromForm] string id)
        {
            return Json(ContactUsService.GetBy(id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تماس با ما کاربران", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserContactUsMainGrid searchInput)
        {
            return Json(ContactUsService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserContactUsMainGrid searchInput)
        {
            var result = ContactUsService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
