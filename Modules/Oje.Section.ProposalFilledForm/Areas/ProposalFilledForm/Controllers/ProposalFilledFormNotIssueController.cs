using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.ProposalFilledForm.Areas.ProposalFilledForm.Controllers
{
    [Area("ProposalFilledForm")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "فرم های پیشنهاد", Icon = "fa-file-powerpoint", Title = "لیست فرم پیشنهاد رد شده")]
    [CustomeAuthorizeFilter]
    public class ProposalFilledFormNotIssueController : Controller
    {
        readonly IProposalFilledFormAdminService ProposalFilledFormAdminService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IProposalFormCategoryService ProposalFormCategoryService = null;
        readonly ICompanyService CompanyService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IRoleService RoleService = null;
        readonly IUserService UserService = null;
        readonly IProposalFilledFormCompanyService ProposalFilledFormCompanyService = null;
        readonly IProposalFilledFormDocumentService ProposalFilledFormDocumentService = null;
        readonly IBankService BankService = null;
        readonly IProposalFilledFormStatusLogService ProposalFilledFormStatusLogService = null;

        public ProposalFilledFormNotIssueController(
            IProposalFilledFormAdminService ProposalFilledFormAdminService,
            AccountService.Interfaces.ISiteSettingService SiteSettingService,
            ICompanyService CompanyService,
            IProposalFormCategoryService ProposalFormCategoryService,
            IProposalFormService ProposalFormService,
            IRoleService RoleService,
            IUserService UserService,
            IProposalFilledFormCompanyService ProposalFilledFormCompanyService,
            IProposalFilledFormDocumentService ProposalFilledFormDocumentService,
            IBankService BankService,
            IProposalFilledFormStatusLogService ProposalFilledFormStatusLogService
            )
        {
            this.ProposalFilledFormAdminService = ProposalFilledFormAdminService;
            this.SiteSettingService = SiteSettingService;
            this.CompanyService = CompanyService;
            this.ProposalFormCategoryService = ProposalFormCategoryService;
            this.ProposalFormService = ProposalFormService;
            this.RoleService = RoleService;
            this.UserService = UserService;
            this.ProposalFilledFormCompanyService = ProposalFilledFormCompanyService;
            this.ProposalFilledFormDocumentService = ProposalFilledFormDocumentService;
            this.BankService = BankService;
            this.ProposalFilledFormStatusLogService = ProposalFilledFormStatusLogService;
        }

        [AreaConfig(Title = "لیست فرم پیشنهاد رد شده", Icon = "fa-print-slash", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست فرم پیشنهاد رد شده";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFilledFormNotIssue", new { area = "ProposalFilledForm" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست فرم پیشنهاد رد شده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledForm", "ProposalFilledFormNotIssue")));
        }

        [AreaConfig(Title = "ویرایش فرم پیشنهاد رد شده", Icon = "fa-pen")]
        [HttpGet]
        public IActionResult Edit(int fid)
        {
            ViewBag.Title = "ویرایش فرم پیشنهاد رد شده";
            ViewBag.exteraParameters = new { fid = fid };
            ViewBag.ConfigRoute = Url.Action("GetEditJsonConfig", "ProposalFilledFormNotIssue", new { area = "ProposalFilledForm" });
            return View("Index");
        }

        [AreaConfig(Title = "تنظیمات صفحه ویرایش فرم پیشنهاد رد شده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetEditJsonConfig(int fid)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(ProposalFilledFormAdminService.GetJsonConfir(
                    fid, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue,
                    Url.Action("GetById", "ProposalFilledFormNotIssue", new { area = "ProposalFilledForm", id = fid }),
                    Url.Action("Update", "ProposalFilledFormNotIssue", new { area = "ProposalFilledForm", id = fid })
                    )
                );
        }

        [AreaConfig(Title = "ذخیره جزییات فرم پیشنهاد رد شده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromQuery] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.Update(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue, Request.Form));
        }


        [AreaConfig(Title = "ذخیره ارجاع برای فرم پیشنهاد رد شده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult CreateUserRefer([FromForm] CreateUpdateProposalFilledFormUserReffer input)
        {
            return Json(ProposalFilledFormAdminService.CreateUserRefer(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "تغییر نماینده فرم پیشنهاد رد شده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateAgent([FromForm] long? userId, long? id)
        {
            return Json(ProposalFilledFormAdminService.UpdateAgent(id, userId, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "مشاهده جزییات فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromQuery] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "حذف فرم پیشنهاد رد شده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "مشاهده وضعیت فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetStatus([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.GetStatus(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "تغییر وضعیت فرم پیشنهاد رد شده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateStatus([FromForm] ProposalFilledFormChangeStatusVM input)
        {
            return Json(ProposalFilledFormAdminService.UpdateStatus(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست نقش ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetRoleList()
        {
            return Json(RoleService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست بانک", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetBankList()
        {
            return Json(BankService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست کاربران جهت ارجاع", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetUsers([FromQuery] Select2SearchVM searchInput, [FromQuery] int? roleId, [FromQuery] int? companyId, [FromQuery] int? provinceId, [FromQuery] int? cityId)
        {
            return Json(UserService.GetSelect2List(searchInput, roleId, companyId, provinceId, cityId, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران ارجاع داده شده فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetRefferUsers([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.GetRefferUsers(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "مشاهده نماینده فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetAgent([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.GetAgent(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPPFCompanies([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.GetCompanies(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "به روز رسانی شرکت فرم پیشنهاد رد شده", Icon = "fa-pencil")]
        [HttpPost]
        public ActionResult UpdateCompanies([FromForm] CreateUpdateProposalFilledFormCompany input)
        {
            return Json(ProposalFilledFormAdminService.UpdateCompanies(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی فرم پیشنهاد", Icon = "fa-eye")]
        [HttpGet]
        public ActionResult GetProposalFormCategoryList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormCategoryService.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? ppfCatId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, ppfCatId));
        }

        [AreaConfig(Title = "افزودن شرکت فرم پیشنهاد رد شده", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreatePricingCompany([FromForm] CreateUpdateProposalFilledFormCompanyPrice input)
        {
            return Json(ProposalFilledFormCompanyService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "ویرایش شرکت فرم پیشنهاد رد شده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdatePricingCompany([FromForm] CreateUpdateProposalFilledFormCompanyPrice input)
        {
            return Json(ProposalFilledFormCompanyService.Update(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "دانلود پی دی اف فرم پیشنهاد رد شده", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("PdfDetailesForAdmin", "ProposalFilledFormNotIssue", new { area = "ProposalFilledForm", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [AreaConfig(Title = "مشاهده اسناد فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPPFImageList([FromForm] GlobalGridParentLong input)
        {
            return Json(ProposalFilledFormAdminService.GetUploadImages(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "حذف اسناد فرم پیشنهاد رد شده", Icon = "fa-trash-o")]
        [HttpPost]
        public ActionResult DeletePPFImage([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(ProposalFilledFormAdminService.DeleteUploadImage(input?.id, pKey, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "افزودن اسناد فرم پیشنهاد رد شده", Icon = "fa-plus")]
        [HttpPost]
        public ActionResult UploadNewFile([FromForm] long? pKey, [FromForm] IFormFile mainFile)
        {
            return Json(ProposalFilledFormAdminService.UploadImage(pKey, mainFile, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "جزییات فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpGet]
        public IActionResult PdfDetailesForAdmin([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.cName = ControllerContext.ActionDescriptor.ControllerName;
            return View(ProposalFilledFormAdminService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "مشاهده شرکت تایین قیمت فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetPricingCompany([FromForm] GlobalStringId input)
        {
            return Json(ProposalFilledFormCompanyService.GetBy(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "حذف شرکت فرم پیشنهاد رد شده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult DeleteCompany([FromForm] GlobalStringId input)
        {
            return Json(ProposalFilledFormCompanyService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "انتخاب شرکت فرم پیشنهاد رد شده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult SelectCompany([FromForm] GlobalStringId input)
        {
            return Json(ProposalFilledFormCompanyService.Select(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }


        [AreaConfig(Title = "مشاهده لیست تایین قیمت فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPricingCompanyList([FromForm] ProposalFilledFormCompanyPriceMainGrid searchInput)
        {
            return Json(ProposalFilledFormCompanyService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "خروجی اکسل تایین قیمت", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult PricingCompanyExport([FromForm] ProposalFilledFormCompanyPriceMainGrid searchInput)
        {
            var result = ProposalFilledFormCompanyService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFilledFormMainGrid searchInput)
        {
            return Json(ProposalFilledFormAdminService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue, HttpContext.GetLoginUser()?.roles));
        }

        [AreaConfig(Title = "خروجی اکسل فرم پیشنهاد رد شده", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFilledFormMainGrid searchInput)
        {
            var result = ProposalFilledFormAdminService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue, HttpContext.GetLoginUser()?.roles);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "حذف مدرک مالی فرم پیشنهاد رد شده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult DeleteDocument([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(ProposalFilledFormDocumentService.Delete(input?.id, pKey, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "ایجاد مدرک مالی فرم پیشنهاد رد شده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult CreateDocument([FromForm] ProposalFilledFormDocumentCreateUpdateVM input)
        {
            return Json(ProposalFilledFormDocumentService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "ایجاد مدرک مالی فرم پیشنهاد رد شده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateDocument([FromForm] ProposalFilledFormDocumentCreateUpdateVM input)
        {
            return Json(ProposalFilledFormDocumentService.Update(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "حذف مدرک مالی فرم پیشنهاد رد شده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult GetDocument([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(ProposalFilledFormDocumentService.GetBy(input?.id, pKey, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "مشاهده لیست مدارک مالی فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetDocumentList([FromForm] ProposalFilledFormDocumentMainGrid searchInput)
        {
            return Json(ProposalFilledFormDocumentService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue));
        }

        [AreaConfig(Title = "خروجی اکسل مدارک مالی فرم پیشنهاد رد شده", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult GetDocumentListExport([FromForm] ProposalFilledFormDocumentMainGrid searchInput)
        {
            var result = ProposalFilledFormDocumentService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NotIssue);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست تاریخچه وضعیت فرم پیشنهاد رد شده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetStatusLogList([FromForm] ProposalFilledFormLogMainGrid searchInput)
        {
            return Json(ProposalFilledFormStatusLogService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }
    }
}
