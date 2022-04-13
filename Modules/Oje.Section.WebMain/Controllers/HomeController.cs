using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.View;
using Oje.Security.Interfaces;

namespace Oje.Section.WebMain.Areas.WebMain.Controllers
{
    public class HomeController : Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUserService UserService = null;
        readonly IFooterExteraLinkService FooterExteraLinkService = null;
        readonly IFooterGroupExteraLinkService FooterGroupExteraLinkService = null;
        readonly ProposalFormService.Interfaces.IProposalFormCategoryService ProposalFormCategoryService = null;
        readonly ProposalFormService.Interfaces.IProposalFormService ProposalFormService = null;
        readonly IOurObjectService OurObjectService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;

        public HomeController
            (
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService,
                IUserService UserService,
                IFooterExteraLinkService FooterExteraLinkService,
                IFooterGroupExteraLinkService FooterGroupExteraLinkService,
                ProposalFormService.Interfaces.IProposalFormCategoryService ProposalFormCategoryService,
                ProposalFormService.Interfaces.IProposalFormService ProposalFormService,
                IOurObjectService OurObjectService,
                IUploadedFileService UploadedFileService,
                IBlockAutoIpService BlockAutoIpService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
            this.UserService = UserService;
            this.FooterExteraLinkService = FooterExteraLinkService;
            this.FooterGroupExteraLinkService = FooterGroupExteraLinkService;
            this.ProposalFormCategoryService = ProposalFormCategoryService;
            this.ProposalFormService = ProposalFormService;
            this.OurObjectService = OurObjectService;
            this.UploadedFileService = UploadedFileService;
            this.BlockAutoIpService = BlockAutoIpService;
        }

        [Route("/")]
        [Route("[Controller]/[Action]")]
        [HttpGet]
        public IActionResult Index()
        {
            var curSetting = SiteSettingService.GetSiteSetting();
            if (curSetting == null)
                return NotFound();

            GlobalServices.FillSeoInfo(
                  ViewData,
                   curSetting.Title,
                   curSetting.SeoMainPage,
                   Request.Scheme + "://" + Request.Host + "/",
                   Request.Scheme + "://" + Request.Host + "/",
                   WebSiteTypes.website,
                   Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                   null
                   );

            return View();
        }

        [Route("Reminder")]
        [HttpGet]
        public IActionResult Reminder()
        {

            GlobalServices.FillSeoInfo(
                  ViewData,
                   "یادآوری",
                   "با درج اطلاعات خود در قسمت یادآوری دیگر نگران فراموش کردن موعد تمدید بیمه یا اقساط بیمه نامه خود نباشید.",
                   Request.Scheme + "://" + Request.Host + "/Reminder",
                   Request.Scheme + "://" + Request.Host + "/Reminder",
                   WebSiteTypes.website,
                   Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                   null
                   );

            return View();
        }

        [Route("CarThirdPartyInquiry")]
        [HttpGet]
        public IActionResult CarThirdPartyInquiry()
        {

            GlobalServices.FillSeoInfo(
                  ViewData,
                   "خرید بیمه نامه شخص ثالث خودرو",
                   "بیمه نامه شخص ثالث خودرو را در این بخش ارزان تر از همه جا تهیه کنید",
                   Request.Scheme + "://" + Request.Host + "/CarThirdPartyInquiry",
                   Request.Scheme + "://" + Request.Host + "/CarThirdPartyInquiry",
                   WebSiteTypes.website,
                   Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                   null
                   );

            return View();
        }

        [Route("CarBodyInquiry")]
        [HttpGet]
        public IActionResult CarBodyInquiry()
        {

            GlobalServices.FillSeoInfo(
                  ViewData,
                   "خرید بیمه نامه بدنه خودرو",
                   "بیمه نامه بدنه خودرو را در این بخش ارزان تر از همه جا تهیه کنید",
                   Request.Scheme + "://" + Request.Host + "/CarBodyInquiry",
                   Request.Scheme + "://" + Request.Host + "/CarBodyInquiry",
                   WebSiteTypes.website,
                   Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                   null
                   );

            return View();
        }

        [Route("FireInsurance")]
        [HttpGet]
        public IActionResult FireInsurance()
        {

            GlobalServices.FillSeoInfo(
                  ViewData,
                   "خرید بیمه نامه آتش سوزی",
                   "بیمه نامه آتش سوزی را در این بخش ارزان تر از همه جا تهیه کنید",
                   Request.Scheme + "://" + Request.Host + "/FireInsurance",
                   Request.Scheme + "://" + Request.Host + "/FireInsurance",
                   WebSiteTypes.website,
                   Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                   null
                   );

            return View();
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public IActionResult GetLoginModalConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMain", "LoginModal")));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public IActionResult GetOtherInsuranceConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMain", "OtherInsurance")));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public IActionResult GetReminderConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMain", "RemindMe")));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public IActionResult GetAboutUsMainPage()
        {
            return Json(PropertyService.GetBy<AboutUsMainPageVM>(PropertyType.AboutUsMainPage, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public IActionResult GetOurPrideMainPage()
        {
            return Json(PropertyService.GetBy<OurPrideVM>(PropertyType.OurPrideMainPage, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public IActionResult GetFooterDescrption()
        {
            return Json(PropertyService.GetBy<FooterDescrptionVM>(PropertyType.FooterDescrption, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetFooterInfor()
        {
            return Json(UserService.GetUserInfoBy(SiteSettingService.GetSiteSetting()?.UserId));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetFooterSambole()
        {
            return Json(PropertyService.GetBy<FooterSymbolCreateUpdateVM>(PropertyType.FooterSymbol, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetFooterExteraLink()
        {
            return Json(FooterExteraLinkService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetFooterExteraLinkGroup()
        {
            return Json(FooterGroupExteraLinkService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public ActionResult GetProposalFormCategoryList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormCategoryService.GetSelect2List(searchInput));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? ppfCatId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, ppfCatId));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetTopLeftIconList()
        {
            return Json(PropertyService.GetBy<MainPageTopLeftIconVM>(PropertyType.MainPageTopLeftIcon, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetFooterSocialUrl()
        {
            return Json(PropertyService.GetBy<FooterSocialIconCreateUpdateVM>(PropertyType.FooterIcon, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetOurCustomerList()
        {
            return Json(OurObjectService.GetListWeb(SiteSettingService.GetSiteSetting()?.Id, OurObjectType.OurCustomers));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetOurCompanyList()
        {
            return Json(OurObjectService.GetListWeb(SiteSettingService.GetSiteSetting()?.Id, OurObjectType.OurContractCompanies));
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult UploadNewFileForOnlineChat([FromForm] IFormFile mainFile)
        {
            if (HttpContext.GetLoginUser()?.UserId == null || HttpContext.GetLoginUser()?.UserId <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);

            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.UploadFileForOnlineChat, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = UploadedFileService.UploadNewFile(FileType.OnlineFile, mainFile, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id, null, ".jpg,.jpeg,.png,.doc,.pdf", true);
            if (!string.IsNullOrEmpty(tempResult))
                tempResult = GlobalConfig.FileAccessHandlerUrl + tempResult;
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.UploadFileForOnlineChat, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult UploadNewVoiceForOnlineChat([FromForm] IFormFile mainFile)
        {
            if (HttpContext.GetLoginUser()?.UserId == null || HttpContext.GetLoginUser()?.UserId <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.UploadVoiceForOnlineChat, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = UploadedFileService.UploadNewFile(FileType.OnlineFile, mainFile, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id, null, ".ogg,.webm", true);
            if (!string.IsNullOrEmpty(tempResult))
                tempResult = GlobalConfig.FileAccessHandlerUrl + tempResult;
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.UploadVoiceForOnlineChat, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }
    }
}
