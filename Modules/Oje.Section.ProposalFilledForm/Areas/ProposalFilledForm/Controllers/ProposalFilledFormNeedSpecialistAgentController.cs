﻿using Microsoft.AspNetCore.Http;
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
    [AreaConfig(ModualTitle = "مدیریت بیمه نامه ها", Order = 7,  Icon = "fa-file-powerpoint", Title = " پیشنهاد نیاز به کارشناسی")]
    [CustomeAuthorizeFilter]
    public class ProposalFilledFormNeedSpecialistAgentController: Controller
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

        public ProposalFilledFormNeedSpecialistAgentController(
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

        [AreaConfig(Title = " پیشنهاد نیاز به کارشناسی", Icon = "fa-print-search", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = " پیشنهاد نیاز به کارشناسی ";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFilledFormNeedSpecialistAgent", new { area = "ProposalFilledForm" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه  پیشنهاد نیاز به کارشناسی ", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledForm", "ProposalFilledFormNeedSpecialistAgent")));
        }

        [AreaConfig(Title = "ویرایش فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-pen")]
        [HttpGet]
        public IActionResult Edit(int fid)
        {
            ViewBag.Title = "ویرایش فرم پیشنهاد نیاز به کارشناسی نماینده";
            ViewBag.exteraParameters = new { fid = fid };
            ViewBag.ConfigRoute = Url.Action("GetEditJsonConfig", "ProposalFilledFormNeedSpecialistAgent", new { area = "ProposalFilledForm" });
            return View("Index");
        }

        [AreaConfig(Title = "تنظیمات صفحه ویرایش فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetEditJsonConfig(int fid)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(ProposalFilledFormAdminService.GetJsonConfir(
                    fid, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist,
                    Url.Action("GetById", "ProposalFilledFormNeedSpecialistAgent", new { area = "ProposalFilledForm", id = fid }),
                    Url.Action("Update", "ProposalFilledFormNeedSpecialistAgent", new { area = "ProposalFilledForm", id = fid })
                    )
                );
        }

        [AreaConfig(Title = "ذخیره جزییات فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromQuery] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.Update(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist, Request.Form));
        }


        [AreaConfig(Title = "ذخیره ارجاع برای فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult CreateUserRefer([FromForm] CreateUpdateProposalFilledFormUserReffer input)
        {
            return Json(ProposalFilledFormAdminService.CreateUserRefer(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "تغییر نماینده فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateAgent([FromForm] long? userId, long? id, [FromForm] int? cSOWSiteSettingId)
        {
            return Json(ProposalFilledFormAdminService.UpdateAgent(id, userId, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "مشاهده جزییات فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromQuery] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "حذف فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "مشاهده وضعیت فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetStatus([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.GetStatus(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "تغییر وضعیت فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateStatus([FromForm] ProposalFilledFormChangeStatusVM input)
        {
            return Json(ProposalFilledFormAdminService.UpdateStatus(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList(HttpContext.GetLoginUser()?.UserId));
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
        public ActionResult GetUsers([FromQuery] Select2SearchVM searchInput, [FromQuery] int? roleId, [FromQuery] int? companyId, [FromQuery] int? provinceId, [FromQuery] int? cityId, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(UserService.GetSelect2List(searchInput, roleId, companyId, provinceId, cityId, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران ارجاع داده شده فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetRefferUsers([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.GetRefferUsers(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "مشاهده نماینده فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetAgent([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.GetAgent(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPPFCompanies([FromForm] GlobalLongId input)
        {
            return Json(ProposalFilledFormAdminService.GetCompanies(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "به روز رسانی شرکت فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-pencil")]
        [HttpPost]
        public ActionResult UpdateCompanies([FromForm] CreateUpdateProposalFilledFormCompany input)
        {
            return Json(ProposalFilledFormAdminService.UpdateCompanies(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی فرم پیشنهاد", Icon = "fa-eye")]
        [HttpGet]
        public ActionResult GetProposalFormCategoryList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormCategoryService.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? ppfCatId, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, ppfCatId, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "افزودن شرکت فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreatePricingCompany([FromForm] CreateUpdateProposalFilledFormCompanyPrice input)
        {
            return Json(ProposalFilledFormCompanyService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "ویرایش شرکت فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdatePricingCompany([FromForm] CreateUpdateProposalFilledFormCompanyPrice input)
        {
            return Json(ProposalFilledFormCompanyService.Update(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "دانلود پی دی اف فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("PdfDetailesForAdmin", "ProposalFilledFormNeedSpecialistAgent", new { area = "ProposalFilledForm", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [AreaConfig(Title = "مشاهده اسناد فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPPFImageList([FromForm] GlobalGridParentLong input)
        {
            return Json(ProposalFilledFormAdminService.GetUploadImages(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "حذف اسناد فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-trash-o")]
        [HttpPost]
        public ActionResult DeletePPFImage([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(ProposalFilledFormAdminService.DeleteUploadImage(input?.id, pKey, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "افزودن اسناد فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-plus")]
        [HttpPost]
        public ActionResult UploadNewFile([FromForm] long? pKey, [FromForm] IFormFile mainFile)
        {
            return Json(ProposalFilledFormAdminService.UploadImage(pKey, mainFile, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "جزییات فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpGet]
        public IActionResult PdfDetailesForAdmin([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.cName = ControllerContext.ActionDescriptor.ControllerName;
            return View(ProposalFilledFormAdminService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser(isPrint)?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "مشاهده شرکت تعیین قیمت فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetPricingCompany([FromForm] GlobalStringId input)
        {
            return Json(ProposalFilledFormCompanyService.GetBy(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "حذف شرکت فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult DeleteCompany([FromForm] GlobalStringId input)
        {
            return Json(ProposalFilledFormCompanyService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "انتخاب شرکت فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult SelectCompany([FromForm] GlobalStringId input)
        {
            return Json(ProposalFilledFormCompanyService.Select(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }


        [AreaConfig(Title = "مشاهده لیست تعیین قیمت فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPricingCompanyList([FromForm] ProposalFilledFormCompanyPriceMainGrid searchInput)
        {
            return Json(ProposalFilledFormCompanyService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "خروجی اکسل تعیین قیمت", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult PricingCompanyExport([FromForm] ProposalFilledFormCompanyPriceMainGrid searchInput)
        {
            var result = ProposalFilledFormCompanyService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده  پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFilledFormMainGrid searchInput)
        {
            return Json(ProposalFilledFormAdminService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist, HttpContext.GetLoginUser()?.roles));
        }

        [AreaConfig(Title = "خروجی اکسل فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFilledFormMainGrid searchInput)
        {
            var result = ProposalFilledFormAdminService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist, HttpContext.GetLoginUser()?.roles);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "حذف مدرک مالی فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult DeleteDocument([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(ProposalFilledFormDocumentService.Delete(input?.id, pKey, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "ایجاد مدرک مالی فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateDocument([FromForm] ProposalFilledFormDocumentCreateUpdateVM input)
        {
            return Json(ProposalFilledFormDocumentService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "به روز رسانی مدرک مالی فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateDocument([FromForm] ProposalFilledFormDocumentCreateUpdateVM input)
        {
            return Json(ProposalFilledFormDocumentService.Update(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "مشاهده مدرک مالی فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetDocument([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(ProposalFilledFormDocumentService.GetBy(input?.id, pKey, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "مشاهده لیست مدارک مالی فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetDocumentList([FromForm] ProposalFilledFormDocumentMainGrid searchInput)
        {
            return Json(ProposalFilledFormDocumentService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist));
        }

        [AreaConfig(Title = "خروجی اکسل مدارک مالی فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult GetDocumentListExport([FromForm] ProposalFilledFormDocumentMainGrid searchInput)
        {
            var result = ProposalFilledFormDocumentService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.NeedSpecialist);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست تاریخچه وضعیت فرم پیشنهاد نیاز به کارشناسی نماینده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetStatusLogList([FromForm] ProposalFilledFormLogMainGrid searchInput)
        {
            return Json(ProposalFilledFormStatusLogService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }
    }
}
