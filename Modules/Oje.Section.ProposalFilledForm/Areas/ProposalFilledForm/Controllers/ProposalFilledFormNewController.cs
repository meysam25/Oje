using Microsoft.AspNetCore.Http;
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
        readonly IRoleManager RoleManager = null;
        readonly IUserManager UserManager = null;
        readonly IProposalFilledFormCompanyManager ProposalFilledFormCompanyManager = null;
        readonly IProposalFilledFormDocumentManager ProposalFilledFormDocumentManager = null;
        readonly IBankManager BankManager = null;
        readonly IProposalFilledFormStatusLogManager ProposalFilledFormStatusLogManager = null;

        public ProposalFilledFormNewController(
            IProposalFilledFormAdminManager ProposalFilledFormAdminManager,
            AccountManager.Interfaces.ISiteSettingManager SiteSettingManager,
            ICompanyManager CompanyManager,
            IProposalFormCategoryManager ProposalFormCategoryManager,
            IProposalFormManager ProposalFormManager,
            IRoleManager RoleManager,
            IUserManager UserManager,
            IProposalFilledFormCompanyManager ProposalFilledFormCompanyManager,
            IProposalFilledFormDocumentManager ProposalFilledFormDocumentManager,
            IBankManager BankManager,
            IProposalFilledFormStatusLogManager ProposalFilledFormStatusLogManager
            )
        {
            this.ProposalFilledFormAdminManager = ProposalFilledFormAdminManager;
            this.SiteSettingManager = SiteSettingManager;
            this.CompanyManager = CompanyManager;
            this.ProposalFormCategoryManager = ProposalFormCategoryManager;
            this.ProposalFormManager = ProposalFormManager;
            this.RoleManager = RoleManager;
            this.UserManager = UserManager;
            this.ProposalFilledFormCompanyManager = ProposalFilledFormCompanyManager;
            this.ProposalFilledFormDocumentManager = ProposalFilledFormDocumentManager;
            this.BankManager = BankManager;
            this.ProposalFilledFormStatusLogManager = ProposalFilledFormStatusLogManager;
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

        [AreaConfig(Title = "ویرایش فرم پیشنهاد جدید", Icon = "fa-pen")]
        [HttpGet]
        public IActionResult Edit(int fid)
        {
            ViewBag.Title = "ویرایش فرم پیشنهاد جدید";
            ViewBag.exteraParameters = new { fid = fid };
            ViewBag.ConfigRoute = Url.Action("GetEditJsonConfig", "ProposalFilledFormNew", new { area = "ProposalFilledForm" });
            return View("Index");
        }

        [AreaConfig(Title = "تنظیمات صفحه ویرایش فرم پیشنهاد جدید", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetEditJsonConfig(int fid)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(ProposalFilledFormAdminManager.GetJsonConfir(
                    fid, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New,
                    Url.Action("GetById", "ProposalFilledFormNew", new { area = "ProposalFilledForm", id = fid }),
                    Url.Action("Update", "ProposalFilledFormNew", new { area = "ProposalFilledForm", id = fid })
                    )
                );
        }

        [AreaConfig(Title = "ذخیره جزییات فرم پیشنهاد جدید", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromQuery] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminManager.Update(input?.id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New, Request.Form));
        }


        [AreaConfig(Title = "ذخیره ارجاع برای فرم پیشنهاد جدید", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult CreateUserRefer([FromForm] CreateUpdateProposalFilledFormUserReffer input)
        {
            return Json(ProposalFilledFormAdminManager.CreateUserRefer(input, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "تغییر نماینده فرم پیشنهاد جدید", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateAgent([FromForm] long? userId, long? id)
        {
            return Json(ProposalFilledFormAdminManager.UpdateAgent(id, userId, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "مشاهده جزییات فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromQuery] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminManager.GetById(input?.id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "حذف فرم پیشنهاد جدید", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminManager.Delete(input?.id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "مشاهده وضعیت فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetStatus([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminManager.GetStatus(input?.id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "تغییر وضعیت فرم پیشنهاد جدید", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateStatus([FromForm] ProposalFilledFormChangeStatusVM input)
        {
            return Json(ProposalFilledFormAdminManager.UpdateStatus(input, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست نقش ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetRoleList()
        {
            return Json(RoleManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست بانک", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetBankList()
        {
            return Json(BankManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست کاربران جهت ارجاع", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetUsers([FromQuery] Select2SearchVM searchInput, [FromQuery] int? roleId, [FromQuery] int? companyId, [FromQuery] int? provinceId, [FromQuery] int? cityId)
        {
            return Json(UserManager.GetSelect2List(searchInput, roleId, companyId, provinceId, cityId));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران ارجاع داده شده فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetRefferUsers([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminManager.GetRefferUsers(input?.id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "مشاهده نماینده فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetAgent([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminManager.GetAgent(input?.id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPPFCompanies([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminManager.GetCompanies(input?.id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "به روز رسانی شرکت فرم پیشنهاد جدید", Icon = "fa-pencil")]
        [HttpPost]
        public ActionResult UpdateCompanies([FromForm] CreateUpdateProposalFilledFormCompany input)
        {
            return Json(ProposalFilledFormAdminManager.UpdateCompanies(input, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی فرم پیشنهاد", Icon = "fa-eye")]
        [HttpGet]
        public ActionResult GetProposalFormCategoryList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormCategoryManager.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? ppfCatId)
        {
            return Json(ProposalFormManager.GetSelect2List(searchInput, ppfCatId));
        }

        [AreaConfig(Title = "افزودن شرکت فرم پیشنهاد جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreatePricingCompany([FromForm] CreateUpdateProposalFilledFormCompanyPrice input)
        {
            return Json(ProposalFilledFormCompanyManager.Create(input, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "ویرایش شرکت فرم پیشنهاد جدید", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdatePricingCompany([FromForm] CreateUpdateProposalFilledFormCompanyPrice input)
        {
            return Json(ProposalFilledFormCompanyManager.Update(input, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "دانلود پی دی اف فرم پیشنهاد جدید", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("PdfDetailesForAdmin", "ProposalFilledFormNew", new { area = "ProposalFilledForm", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [AreaConfig(Title = "مشاهده اسناد فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPPFImageList([FromForm] GlobalGridParentLong input)
        {
            return Json(ProposalFilledFormAdminManager.GetUploadImages(input, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "حذف اسناد فرم پیشنهاد جدید", Icon = "fa-trash-o")]
        [HttpPost]
        public ActionResult DeletePPFImage([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(ProposalFilledFormAdminManager.DeleteUploadImage(input?.id, pKey, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "افزودن اسناد فرم پیشنهاد جدید", Icon = "fa-plus")]
        [HttpPost]
        public ActionResult UploadNewFile([FromForm] long? pKey, [FromForm] IFormFile mainFile)
        {
            return Json(ProposalFilledFormAdminManager.UploadImage(pKey, mainFile, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "جزییات فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpGet]
        public IActionResult PdfDetailesForAdmin([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            return View(ProposalFilledFormAdminManager.PdfDetailes(id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "مشاهده شرکت تایین قیمت فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetPricingCompany([FromForm] GlobalStringId input)
        {
            return Json(ProposalFilledFormCompanyManager.GetBy(input?.id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "حذف شرکت فرم پیشنهاد جدید", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult DeleteCompany([FromForm] GlobalStringId input)
        {
            return Json(ProposalFilledFormCompanyManager.Delete(input?.id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "انتخاب شرکت فرم پیشنهاد جدید", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult SelectCompany([FromForm] GlobalStringId input)
        {
            return Json(ProposalFilledFormCompanyManager.Select(input?.id, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }


        [AreaConfig(Title = "مشاهده لیست تایین قیمت فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPricingCompanyList([FromForm] ProposalFilledFormCompanyPriceMainGrid searchInput)
        {
            return Json(ProposalFilledFormCompanyManager.GetList(searchInput, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "خروجی اکسل تایین قیمت", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult PricingCompanyExport([FromForm] ProposalFilledFormCompanyPriceMainGrid searchInput)
        {
            var result = ProposalFilledFormCompanyManager.GetList(searchInput, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFilledFormMainGrid searchInput)
        {
            return Json(ProposalFilledFormAdminManager.GetList(searchInput, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "خروجی اکسل فرم پیشنهاد جدید", Icon = "fa-file-excel")]
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

        [AreaConfig(Title = "حذف مدرک مالی فرم پیشنهاد جدید", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult DeleteDocument([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(ProposalFilledFormDocumentManager.Delete(input?.id, pKey, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "ایجاد مدرک مالی فرم پیشنهاد جدید", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult CreateDocument([FromForm] ProposalFilledFormDocumentCreateUpdateVM input)
        {
            return Json(ProposalFilledFormDocumentManager.Create(input, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "ایجاد مدرک مالی فرم پیشنهاد جدید", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateDocument([FromForm] ProposalFilledFormDocumentCreateUpdateVM input)
        {
            return Json(ProposalFilledFormDocumentManager.Update(input, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "حذف مدرک مالی فرم پیشنهاد جدید", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult GetDocument([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(ProposalFilledFormDocumentManager.GetBy(input?.id, pKey, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "مشاهده لیست مدارک مالی فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetDocumentList([FromForm] ProposalFilledFormDocumentMainGrid searchInput)
        {
            return Json(ProposalFilledFormDocumentManager.GetList(searchInput, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }

        [AreaConfig(Title = "خروجی اکسل مدارک مالی فرم پیشنهاد جدید", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult GetDocumentListExport([FromForm] ProposalFilledFormDocumentMainGrid searchInput)
        {
            var result = ProposalFilledFormDocumentManager.GetList(searchInput, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست تاریخچه وضعیت فرم پیشنهاد جدید", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetStatusLogList([FromForm] ProposalFilledFormLogMainGrid searchInput)
        {
            return Json(ProposalFilledFormStatusLogManager.GetList(searchInput, SiteSettingManager.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId));
        }
    }
}
