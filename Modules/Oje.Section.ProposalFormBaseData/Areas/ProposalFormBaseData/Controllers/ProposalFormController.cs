using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Areas.ProposalFormBaseData.Controllers
{
    [Area("ProposalFormBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Icon = "fa-file-powerpoint", Title = "فرم های پیشنهاد")]
    [CustomeAuthorizeFilter]
    public class ProposalFormController: Controller
    {
        readonly IProposalFormManager ProposalFormManager = null;
        readonly ISiteSettingManager SiteSettingManager = null;
        readonly IProposalFormCategoryManager ProposalFormCategoryManager = null;
        public ProposalFormController(IProposalFormManager ProposalFormManager, ISiteSettingManager SiteSettingManager, IProposalFormCategoryManager ProposalFormCategoryManager)
        {
            this.ProposalFormManager = ProposalFormManager;
            this.SiteSettingManager = SiteSettingManager;
            this.ProposalFormCategoryManager = ProposalFormCategoryManager;
        }

        [AreaConfig(Title = "فرم های پیشنهاد", Icon = "fa-file-alt", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فرم های پیشنهاد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalForm", new { area = "ProposalFormBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست فرم های پیشنهاد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormBaseData", "ProposalForm")));
        }

        [AreaConfig(Title = "افزودن فرم های پیشنهاد جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateProposalFormVM input)
        {
            return Json(ProposalFormManager.Create(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "حذف فرم های پیشنهاد", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک فرم های پیشنهاد", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  فرم های پیشنهاد", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateProposalFormVM input)
        {
            return Json(ProposalFormManager.Update(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFormMainGrid searchInput)
        {
            return Json(ProposalFormManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFormMainGrid searchInput)
        {
            var result = ProposalFormManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "لیست وب سایت ها", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetSiteList()
        {
            return Json(SiteSettingManager.GetLightList());
        }

        [AreaConfig(Title = "لیست گروه بندی", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetCategoryList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormCategoryManager.GetSelect2List(searchInput));
        }
    }
}
