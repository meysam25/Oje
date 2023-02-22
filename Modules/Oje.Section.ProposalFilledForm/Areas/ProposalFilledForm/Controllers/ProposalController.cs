using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Models.View;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.Section.ProposalFilledForm.Models.View;
using Oje.Security.Interfaces;

namespace Oje.Section.ProposalFilledForm.Areas.ProposalFilledForm.Controllers
{
    [Area("ProposalFilledForm")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت بیمه نامه ها", Icon = "fa-file-powerpoint", Title = "ثبت فرم")]
    public class ProposalController : Controller
    {
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IProposalFormRequiredDocumentService ProposalFormRequiredDocumentService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IProposalFilledFormService ProposalFilledFormService = null;
        readonly IGlobalInqueryService GlobalInqueryService = null;
        readonly IPaymentMethodService PaymentMethodService = null;
        readonly IBankService BankService = null;
        readonly AccountService.Interfaces.IUserService UserService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly IProposalFilledFormAdminService ProposalFilledFormAdminService = null;
        readonly IColorService ColorService = null;

        public ProposalController(
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IProposalFormRequiredDocumentService ProposalFormRequiredDocumentService,
                IProposalFormService ProposalFormService,
                IProposalFilledFormService ProposalFilledFormService,
                IGlobalInqueryService GlobalInqueryService,
                IPaymentMethodService PaymentMethodService,
                IBankService BankService,
                AccountService.Interfaces.IUserService UserService,
                IBlockAutoIpService BlockAutoIpService,
                IProposalFilledFormAdminService ProposalFilledFormAdminService,
                IColorService ColorService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormRequiredDocumentService = ProposalFormRequiredDocumentService;
            this.ProposalFormService = ProposalFormService;
            this.ProposalFilledFormService = ProposalFilledFormService;
            this.GlobalInqueryService = GlobalInqueryService;
            this.PaymentMethodService = PaymentMethodService;
            this.BankService = BankService;
            this.UserService = UserService;
            this.BlockAutoIpService = BlockAutoIpService;
            this.ProposalFilledFormAdminService = ProposalFilledFormAdminService;
            this.ColorService = ColorService;
        }

        [AreaConfig(Title = "ثبت فرم", Icon = "fa-file-powerpoint")]
        [HttpGet]
        public IActionResult Form([FromQuery] ProposalFormVM input)
        {
            
            ViewBag.Title = "ثبت فرم";
            ViewBag.exteraParameters = input;
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Proposal", new { area = "ProposalFilledForm" });
            ViewBag.fid = input?.fid;
           

            return View("Index");
        }

        [AreaConfig(Title = "تنظیمات صفحه ثبت فرم", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig([FromForm] ProposalFormVM input)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(ProposalFormService.GetJSonConfigFile(input.fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "ذخیره فرم پیشنهاد جدید", Icon = "fa-pen")]
        [HttpPost]
        public IActionResult Create()
        {
            var loginUser = HttpContext.GetLoginUser();
            if (loginUser == null || loginUser.UserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateProposalFilledForm, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = ProposalFilledFormService.Create(SiteSettingService.GetSiteSetting()?.Id, Request.Form, loginUser.UserId, Request.GetTargetAreaByRefferForPPFDetailes(), loginUser);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateProposalFilledForm, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [AreaConfig(Title = "پیش نمایش فرم پیشنهاد", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult PrintPreview()
        {
            var loginUserId = HttpContext.GetLoginUser()?.UserId;
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            ProposalFilledFormService.JustValidation(SiteSettingService.GetSiteSetting()?.Id, Request.Form, loginUserId, Request.GetTargetAreaByRefferForPPFDetailes());
            ViewBag.makeLayoutNull = true;
            return View("PdfDetailes", ProposalFilledFormAdminService.PdfDetailesByForm(Request.Form, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        public IActionResult GetTermsHtml(int? fid)
        {
            var foundPPF = ProposalFormService.GetById(fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id);
            if (foundPPF == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var ss = SiteSettingService.GetSiteSetting();

            ViewBag.ContractFile = foundPPF.ContractFile;
            ViewBag.RulesFile = foundPPF.RulesFile;
            ViewBag.HtmlTemplate = foundPPF.TermTemplate;
            ViewBag.companyTitle = ss?.Title;
            ViewBag.pTitle = foundPPF.Title;
            ViewBag.domain = "http" + (ss.IsHttps ? "s" : "") + "://" + ss?.WebsiteUrl;

            return View();
        }

        [HttpPost]
        public IActionResult GetPPFCondationFile([FromForm] int? fid)
        {
            var foundPPF = ProposalFormService.GetById(fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id);
            if (foundPPF == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var ss = SiteSettingService.GetSiteSetting();

            return Content("http" + (ss.IsHttps ? "s" : "") + "://" + ss?.WebsiteUrl + Infrastructure.GlobalConfig.FileAccessHandlerUrl + foundPPF.RulesFile);
        }

        [HttpPost]
        public IActionResult GetPPFContractFile([FromForm] int? fid)
        {
            var foundPPF = ProposalFormService.GetById(fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id);
            if (foundPPF == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var ss = SiteSettingService.GetSiteSetting();

            return Content("http" + (ss.IsHttps ? "s" : "") + "://" + ss?.WebsiteUrl + Infrastructure.GlobalConfig.FileAccessHandlerUrl + foundPPF.ContractFile);
        }

        [AreaConfig(Title = "لیست مدارم مورد نیاز فرم پیشنهاد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetProposalFormRequiredDocument([FromForm] int? fid)
        {
            return Json(ProposalFormRequiredDocumentService.GetLightList(SiteSettingService.GetSiteSetting()?.Id, fid));
        }

        [AreaConfig(Title = "آیا حالت اقساطی فعال می باشد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult CanShowDebitPaymentStep([FromForm] ProposalFormVM input)
        {
            return Json(GlobalInqueryService.GetSumPrice(input.inquiryId.ToLongReturnZiro(), input.fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست شرایط پرداخت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetDebitPaymentCondationList([FromForm] ProposalFormVM input)
        {
            return Json(
                        PaymentMethodService.GetLightList(input.fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id,
                            GlobalInqueryService.GetCompanyId(input.inquiryId.ToLongReturnZiro(), SiteSettingService.GetSiteSetting()?.Id))
                    );
        }

        [AreaConfig(Title = "مشاهده جزئیات شرایط پرداخت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetDebitPaymentCondationDetailes([FromForm] ProposalFormVM input, [FromForm] int id)
        {
            return Json(
                    PaymentMethodService.GetItemDetailes(id, SiteSettingService.GetSiteSetting()?.Id,
                        GlobalInqueryService.GetSumPriceLong(input.inquiryId.ToLongReturnZiro(), input.fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id), input.fid.ToIntReturnZiro())
                    );
        }

        [AreaConfig(Title = "مشاهده لیست بانک", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetBankList()
        {
            return Json(BankService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست رنگ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetColorList()
        {
            return Json(ColorService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست نماینده", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetAgentList([FromQuery] Select2SearchVM searchInput, [FromQuery] ProposalFormVM input, [FromQuery] ProvinceAndCityVM provinceAndCityInput, [FromQuery] string mapLat, [FromQuery] string mapLon, [FromQuery] string cIds)
        {
            return Json(
                    UserService.GetSelect2ListByPPFAndCompanyId(searchInput, SiteSettingService.GetSiteSetting()?.Id, input.fid.ToIntReturnZiro(),
                            (!string.IsNullOrEmpty(cIds)  ? cIds.ToIntReturnZiro() : GlobalInqueryService.GetCompanyId(input.inquiryId.ToLongReturnZiro(), SiteSettingService.GetSiteSetting()?.Id)), 
                            provinceAndCityInput, mapLat, mapLon)
                    );
        }
    }
}
