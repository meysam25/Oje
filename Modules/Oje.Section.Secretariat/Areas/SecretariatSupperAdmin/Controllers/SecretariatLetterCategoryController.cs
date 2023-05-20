using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Secretariat.Interfaces;
using Oje.Section.Secretariat.Models.View;
using System;

namespace Oje.Section.Secretariat.Areas.SecretariatSupperAdmin.Controllers
{
    [Area("SecretariatSupperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات دبیر خانه", Icon = "fa-typewriter", Title = "گروه بندی")]
    [CustomeAuthorizeFilter]
    public class SecretariatLetterCategoryController: Controller
    {
        readonly ISecretariatLetterCategoryService SecretariatLetterCategoryService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ISecretariatHeaderFooterService SecretariatHeaderFooterService = null;
        readonly ISecretariatHeaderFooterDescriptionService SecretariatHeaderFooterDescriptionService = null;
        public SecretariatLetterCategoryController
            (
                ISecretariatLetterCategoryService SecretariatLetterCategoryService,
                ISiteSettingService SiteSettingService,
                ISecretariatHeaderFooterService SecretariatHeaderFooterService,
                ISecretariatHeaderFooterDescriptionService SecretariatHeaderFooterDescriptionService
            )
        {
            this.SecretariatLetterCategoryService = SecretariatLetterCategoryService;
            this.SiteSettingService = SiteSettingService;
            this.SecretariatHeaderFooterService = SecretariatHeaderFooterService;
            this.SecretariatHeaderFooterDescriptionService = SecretariatHeaderFooterDescriptionService;
        }

        [AreaConfig(Title = "گروه بندی", Icon = "fa-palette", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گروه بندی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SecretariatLetterCategory", new { area = "SecretariatSupperAdmin" });

            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست گروه بندی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SecretariatSupperAdmin", "SecretariatLetterCategory")));
        }

        [AreaConfig(Title = "افزودن گروه بندی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SecretariatLetterCategoryCreateUpdateVM input)
        {
            return Json(SecretariatLetterCategoryService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف گروه بندی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SecretariatLetterCategoryService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک گروه بندی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SecretariatLetterCategoryService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  گروه بندی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SecretariatLetterCategoryCreateUpdateVM input)
        {
            return Json(SecretariatLetterCategoryService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SecretariatLetterCategoryMainGrid searchInput)
        {
            return Json(SecretariatLetterCategoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SecretariatLetterCategoryMainGrid searchInput)
        {
            var result = SecretariatLetterCategoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست هدر و فوتر", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetHeaderFoterList()
        {
            return Json(SecretariatHeaderFooterService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست توضیحات هدر و فوتر", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetHeaderFoterDescriptionList([FromForm] SecretariatLetterCategoryMainGrid searchInput)
        {
            return Json(SecretariatHeaderFooterDescriptionService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
