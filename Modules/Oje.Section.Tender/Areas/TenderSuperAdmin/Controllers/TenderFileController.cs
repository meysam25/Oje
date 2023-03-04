using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Models.View;
using System;
using Oje.AccountService.Interfaces;
using Oje.Section.Tender.Interfaces;

namespace Oje.Section.Tender.Areas.TenderSuperAdmin.Controllers
{
    [Area("TenderSuperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه مناقصات", Icon = "fa-funnel-dollar", Title = "دستورالعمل")]
    [CustomeAuthorizeFilter]
    public class TenderFileController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly ITenderFileService TenderFileService = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;
        public TenderFileController(
            ISiteSettingService SiteSettingService,
            ITenderFileService TenderFileService,
            IUserRegisterFormService UserRegisterFormService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.TenderFileService = TenderFileService;
            this.UserRegisterFormService = UserRegisterFormService;
        }

        [AreaConfig(Title = "دستورالعمل", Icon = "fa-file-invoice", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "دستورالعمل";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TenderFile", new { area = "TenderSuperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست دستورالعمل", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("TenderSuperAdmin", "TenderFile")));
        }

        [AreaConfig(Title = "افزودن دستورالعمل جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] TenderFileCreateUpdateVM input)
        {
            return Json(TenderFileService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف دستورالعمل", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(TenderFileService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک دستورالعمل", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(TenderFileService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  دستورالعمل", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] TenderFileCreateUpdateVM input)
        {
            return Json(TenderFileService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست دستورالعمل", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] TenderFileMainGrid searchInput)
        {
            return Json(TenderFileService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] TenderFileMainGrid searchInput)
        {
            var result = TenderFileService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }


        [AreaConfig(Title = "مشاهده لیست فرم های ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetFormList()
        {
            return Json(UserRegisterFormService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
