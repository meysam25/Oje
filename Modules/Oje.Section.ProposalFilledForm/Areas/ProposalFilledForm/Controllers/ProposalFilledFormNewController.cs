using Microsoft.AspNetCore.Mvc;
using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.View;
using System;

namespace Oje.Section.ProposalFilledForm.Areas.ProposalFilledForm.Controllers
{
    [Area("ProposalFilledForm")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "فرم های پیشنهاد", Icon = "fa-file-powerpoint", Title = "لیست فرم پیشنهاد جدید")]
    [CustomeAuthorizeFilter]
    public class ProposalFilledFormNewController : Controller
    {
        readonly IProposalFilledFormAdminManager ProposalFilledFormAdminManager = null;
        readonly AccountManager.Interfaces.ISiteSettingManager SiteSettingManager = null;
        readonly IProposalFormCategoryManager ProposalFormCategoryManager = null;
        readonly ICompanyManager CompanyManager = null;
        readonly IProposalFormManager ProposalFormManager = null;

        public ProposalFilledFormNewController(
            IProposalFilledFormAdminManager ProposalFilledFormAdminManager,
            AccountManager.Interfaces.ISiteSettingManager SiteSettingManager,
            ICompanyManager CompanyManager,
            IProposalFormCategoryManager ProposalFormCategoryManager,
            IProposalFormManager ProposalFormManager
            )
        {
            this.ProposalFilledFormAdminManager = ProposalFilledFormAdminManager;
            this.SiteSettingManager = SiteSettingManager;
            this.CompanyManager = CompanyManager;
            this.ProposalFormCategoryManager = ProposalFormCategoryManager;
            this.ProposalFormManager = ProposalFormManager;
        }

        [AreaConfig(Title = "لیست فرم پیشنهاد جدید", Icon = "fa-file-o", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست فرم پیشنهاد جدید";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFilledFormNew", new { area = "ProposalFilledForm" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست فرم پیشنهاد جدید", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledForm", "ProposalFilledFormNew")));
        }

        [AreaConfig(Title = "حذف فرم پیشنهاد جدید", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminManager.Delete(input?.id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "مشاهده لیست فرم پیشنهاد جدید", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFilledFormMainGrid searchInput)
        {
            return Json(ProposalFilledFormAdminManager.GetList(searchInput, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ها", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی فرم پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetProposalFormCategoryList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormCategoryManager.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست فرم پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? ppfCatId)
        {
            return Json(ProposalFormManager.GetSelect2List(searchInput, ppfCatId));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFilledFormMainGrid searchInput)
        {
            var result = ProposalFilledFormAdminManager.GetList(searchInput, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
