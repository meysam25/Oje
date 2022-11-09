using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.View;
using System;
using Oje.Section.GlobalForms.Services;

namespace Oje.Section.GlobalForms.Areas.GlobalFormAdmin.Controllers
{
    [Area("GlobalFormAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "فرم های عمومی", Icon = "fa-file-powerpoint", Title = "دسترسی فرم عمومی")]
    [CustomeAuthorizeFilter]
    public class GeneralFormStatusRoleController: Controller
    {
        readonly IGeneralFormStatusRoleService GeneralFormStatusRoleService = null;
        readonly IGeneralFormStatusService GeneralFormStatusService = null;
        readonly IRoleService RoleService = null;
        readonly IGeneralFormService GeneralFormService = null;


        public GeneralFormStatusRoleController
            (
                IGeneralFormStatusRoleService GeneralFormStatusRoleService,
                IGeneralFormStatusService GeneralFormStatusService,
                IRoleService RoleService,
                IGeneralFormService GeneralFormService
            )
        {
            this.GeneralFormStatusRoleService = GeneralFormStatusRoleService;
            this.GeneralFormStatusService = GeneralFormStatusService;
            this.RoleService = RoleService;
            this.GeneralFormService = GeneralFormService;
        }

        [AreaConfig(Title = "دسترسی فرم عمومی", Icon = "fa-eye", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "دسترسی فرم عمومی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "GeneralFormStatusRole", new { area = "GlobalFormAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست دسترسی فرم عمومی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("GlobalFormAdmin", "GeneralFormStatusRole")));
        }

        [AreaConfig(Title = "افزودن دسترسی فرم عمومی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] GeneralFormStatusRoleCreateUpdateVM input)
        {
            return Json(GeneralFormStatusRoleService.Create(input));
        }

        [AreaConfig(Title = "حذف دسترسی فرم عمومی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(GeneralFormStatusRoleService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک دسترسی فرم عمومی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(GeneralFormStatusRoleService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  دسترسی فرم عمومی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] GeneralFormStatusRoleCreateUpdateVM input)
        {
            return Json(GeneralFormStatusRoleService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست دسترسی فرم عمومی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] GeneralFormStatusRoleMainGrid searchInput)
        {
            return Json(GeneralFormStatusRoleService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] GeneralFormStatusRoleMainGrid searchInput)
        {
            var result = GeneralFormStatusRoleService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }


        [AreaConfig(Title = "مشاهده لیست فرم", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(GeneralFormService.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست وضعیت", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetStatusList([FromQuery] Select2SearchVM searchInput, [FromQuery] long? fid)
        {
            return Json(GeneralFormStatusService.GetSelect2List(searchInput, fid));
        }

        [AreaConfig(Title = "مشاهده لیست نقش", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetRoleList()
        {
            return Json(RoleService.GetLightList());
        }
    }
}
