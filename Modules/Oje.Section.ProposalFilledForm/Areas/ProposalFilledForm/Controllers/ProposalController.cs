using Microsoft.AspNetCore.Mvc;
using Oje.AccountManager.Filters;
using Oje.AccountManager.Models.View;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.Section.ProposalFilledForm.Models.View;

namespace Oje.Section.ProposalFilledForm.Areas.ProposalFilledForm.Controllers
{
    [Area("ProposalFilledForm")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "فرم های پیشنهاد", Icon = "fa-file-powerpoint", Title = "ثبت فرم")]
    [CustomeAuthorizeFilter]
    public class ProposalController : Controller
    {
        readonly AccountManager.Interfaces.ISiteSettingManager SiteSettingManager = null;
        readonly IProposalFormRequiredDocumentManager ProposalFormRequiredDocumentManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        readonly IProposalFilledFormManager ProposalFilledFormManager = null;
        readonly IGlobalInqueryManager GlobalInqueryManager = null;
        readonly IPaymentMethodManager PaymentMethodManager = null;
        readonly IBankManager BankManager = null;
        readonly AccountManager.Interfaces.IUserManager UserManager = null;
        public ProposalController(
                AccountManager.Interfaces.ISiteSettingManager SiteSettingManager,
                IProposalFormRequiredDocumentManager ProposalFormRequiredDocumentManager,
                IProposalFormManager ProposalFormManager,
                IProposalFilledFormManager ProposalFilledFormManager,
                IGlobalInqueryManager GlobalInqueryManager,
                IPaymentMethodManager PaymentMethodManager,
                IBankManager BankManager,
                AccountManager.Interfaces.IUserManager UserManager
            )
        {
            this.SiteSettingManager = SiteSettingManager;
            this.ProposalFormRequiredDocumentManager = ProposalFormRequiredDocumentManager;
            this.ProposalFormManager = ProposalFormManager;
            this.ProposalFilledFormManager = ProposalFilledFormManager;
            this.GlobalInqueryManager = GlobalInqueryManager;
            this.PaymentMethodManager = PaymentMethodManager;
            this.BankManager = BankManager;
            this.UserManager = UserManager;
        }

        [AreaConfig(Title = "ثبت فرم", Icon = "fa-file-powerpoint")]
        [HttpGet]
        public IActionResult Form([FromQuery] ProposalFormVM input)
        {
            ViewBag.Title = "ثبت فرم";
            ViewBag.exteraParameters = input;
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Proposal", new { area = "ProposalFilledForm" });
            return View("Index");
        }

        [AreaConfig(Title = "تنظیمات صفحه ثبت فرم", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig([FromForm] ProposalFormVM input)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(ProposalFormManager.GetJSonConfigFile(input.fid.ToIntReturnZiro(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "ذخیره فرم پیشنهاد جدید", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult Create()
        {
            return Json(ProposalFilledFormManager.Create(SiteSettingManager.GetSiteSetting()?.Id, Request.Form, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "لیست مدارم مورد نیاز فرم پیشنهاد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetProposalFormRequiredDocument([FromForm] int? fid)
        {
            return Json(ProposalFormRequiredDocumentManager.GetLightList(SiteSettingManager.GetSiteSetting()?.Id, fid));
        }

        [AreaConfig(Title = "آیا حالت اقساطی فعال می باشد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult CanShowDebitPaymentStep([FromForm] ProposalFormVM input)
        {
            return Json(GlobalInqueryManager.GetSumPrice(input.inquiryId.ToLongReturnZiro(), input.fid.ToIntReturnZiro(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست شرایط پرداخت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetDebitPaymentCondationList([FromForm] ProposalFormVM input)
        {
            return Json(
                        PaymentMethodManager.GetLightList(input.fid.ToIntReturnZiro(), SiteSettingManager.GetSiteSetting()?.Id,
                            GlobalInqueryManager.GetCompanyId(input.inquiryId.ToLongReturnZiro(), SiteSettingManager.GetSiteSetting()?.Id))
                    );
        }

        [AreaConfig(Title = "مشاهده جزئیات شرایط پرداخت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetDebitPaymentCondationDetailes([FromForm] ProposalFormVM input, [FromForm] int id)
        {
            return Json(
                    PaymentMethodManager.GetItemDetailes(id, SiteSettingManager.GetSiteSetting()?.Id,
                        GlobalInqueryManager.GetSumPriceLong(input.inquiryId.ToLongReturnZiro(), input.fid.ToIntReturnZiro(), SiteSettingManager.GetSiteSetting()?.Id), input.fid.ToIntReturnZiro())
                    );
        }

        [AreaConfig(Title = "مشاهده لیست بانک", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetBankList()
        {
            return Json(BankManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست نماینده", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetAgentList([FromQuery] Select2SearchVM searchInput, [FromQuery] ProposalFormVM input, [FromQuery] ProvinceAndCityVM provinceAndCityInput)
        {
            return Json(
                    UserManager.GetSelect2ListByPPFAndCompanyId(searchInput, SiteSettingManager.GetSiteSetting()?.Id, input.fid.ToIntReturnZiro(),
                            GlobalInqueryManager.GetCompanyId(input.inquiryId.ToLongReturnZiro(), SiteSettingManager.GetSiteSetting()?.Id), provinceAndCityInput)
                    );
        }
    }
}
