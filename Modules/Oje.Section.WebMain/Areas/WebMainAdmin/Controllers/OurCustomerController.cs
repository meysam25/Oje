using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Models.View;
using System;
using Oje.Infrastructure.Exceptions;
using IOurObjectService = Oje.Section.WebMain.Interfaces.IOurObjectService;

namespace Oje.Section.WebMain.Areas.WebMainAdmin.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات وب سایت", Icon = "fa-browser", Title = "مشتریان ما")]
    [CustomeAuthorizeFilter]
    public class OurCustomerController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IOurObjectService OurObjectService = null;
        public OurCustomerController
            (
                ISiteSettingService SiteSettingService,
                IOurObjectService OurObjectService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.OurObjectService = OurObjectService;
        }

        [AreaConfig(Title = "مشتریان ما", Icon = "fa-user-crown", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مشتریان ما";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "OurCustomer", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مشتریان ما", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "OurCustomer")));
        }

        [AreaConfig(Title = "افزودن مشتریان ما جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] OurCustomerCreateUpdateVM input)
        {
            return Json(OurObjectService.Create(input, SiteSettingService.GetSiteSetting()?.Id, OurObjectType.OurCustomers));
        }

        [AreaConfig(Title = "حذف مشتریان ما", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(OurObjectService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id, OurObjectType.OurCustomers));
        }

        [AreaConfig(Title = "مشاهده  یک مشتریان ما", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(OurObjectService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id, OurObjectType.OurCustomers));
        }

        [AreaConfig(Title = "به روز رسانی  مشتریان ما", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] OurCustomerCreateUpdateVM input)
        {
            return Json(OurObjectService.Update(input, SiteSettingService.GetSiteSetting()?.Id, OurObjectType.OurCustomers));
        }

        [AreaConfig(Title = "مشاهده لیست مشتریان ما", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] OurCustomerMainGrid searchInput)
        {
            return Json(OurObjectService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, OurObjectType.OurCustomers));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] OurCustomerMainGrid searchInput)
        {
            var result = OurObjectService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, OurObjectType.OurCustomers);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
