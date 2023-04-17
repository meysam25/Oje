using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sanab.Interfaces;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.View;
using Oje.Security.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using IOurObjectService = Oje.Section.WebMain.Interfaces.IOurObjectService;
using IUpdateAllSignatures = Oje.ValidatedSignature.Interfaces.IUpdateAllSignatures;

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
        readonly IExternalNotificationServicePushSubscriptionService ExternalNotificationServicePushSubscriptionService = null;
        readonly IShortLinkService ShortLinkService = null;

        readonly IUserInquiry UserInquiry = null;
        readonly ICarInquiry CarInquiry = null;
        readonly IUpdateAllSignatures UpdateAllSignatures = null;

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
                IBlockAutoIpService BlockAutoIpService,
                IExternalNotificationServicePushSubscriptionService ExternalNotificationServicePushSubscriptionService,
                IShortLinkService ShortLinkService,
                IUserInquiry UserInquiry,
                ICarInquiry CarInquiry,
                IUpdateAllSignatures UpdateAllSignatures
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
            this.ExternalNotificationServicePushSubscriptionService = ExternalNotificationServicePushSubscriptionService;
            this.ShortLinkService = ShortLinkService;
            this.UserInquiry = UserInquiry;
            this.CarInquiry = CarInquiry;
            this.UpdateAllSignatures = UpdateAllSignatures;
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public IActionResult Index()
        {
            

            var curSetting = SiteSettingService.GetSiteSetting();
            if (curSetting == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            GlobalServices.FillSeoInfo(
                  ViewData,
                   curSetting.Title,
                   curSetting.SeoMainPage,
                   Request.Scheme + "://" + Request.Host + "/",
                   Request.Scheme + "://" + Request.Host + "/",
                   WebSiteTypes.website,
                   Request.Scheme + "://" + Request.Host + GlobalConfig.FileAccessHandlerUrl + curSetting.Image512,
                   null,
                   LdJsonService.GetCorporation(
                        curSetting.Title,
                        curSetting.WebsiteUrl.Split('.')[0],
                        Request.Scheme + "://" + Request.Host + "/",
                        Request.Scheme + "://" + Request.Host + GlobalConfig.FileAccessHandlerUrl + curSetting.Image512,
                        curSetting.User?.Email,
                        new List<Dictionary<string, object>>()
                        {
                            LdJsonService.GetContactTell(curSetting.User?.Tell),
                            LdJsonService.GetAddress
                                (
                                    curSetting.User?.Address,
                                    curSetting.User?.Province?.Title + " " + curSetting.User?.City?.Title,
                                    curSetting.User?.PostalCode,
                                    LdJsonService.GetGEO(curSetting.User?.MapLocation?.X, curSetting.User?.MapLocation?.Y)
                                ),
                            LdJsonService.GetFounder(curSetting.User?.Firstname + " " + curSetting.User?.Lastname, ( !string.IsNullOrEmpty(curSetting.User?.UserPic) ? GlobalConfig.FileAccessHandlerUrl + curSetting.User?.UserPic : "" ) )
                        }
                       )
                   );

            //PushNotificationService.Send();

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
        [HttpGet]
        public IActionResult GetLoginModalConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMain", "LoginModal")));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public IActionResult GetOtherInsuranceConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMain", "OtherInsurance")));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public IActionResult GetReminderConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMain", "RemindMe")));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public IActionResult GetAboutUsMainPage()
        {
            return Json(PropertyService.GetBy<AboutUsMainPageVM>(PropertyType.AboutUsMainPage, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public IActionResult GetOurPrideMainPage()
        {
            return Json(PropertyService.GetBy<OurPrideVM>(PropertyType.OurPrideMainPage, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public IActionResult GetFooterDescrption()
        {
            return Json(PropertyService.GetBy<FooterDescrptionVM>(PropertyType.FooterDescrption, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public ActionResult GetFooterInfor()
        {
            return Json(UserService.GetUserInfoBy(SiteSettingService.GetSiteSetting()?.UserId));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public ActionResult GetFooterSambole()
        {
            return Json(PropertyService.GetBy<FooterSymbolCreateUpdateVM>(PropertyType.FooterSymbol, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public ActionResult GetFooterExteraLink()
        {
            return Json(FooterExteraLinkService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
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
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? ppfCatId, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, ppfCatId, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public ActionResult GetTopLeftIconList()
        {
            return Json(PropertyService.GetBy<MainPageTopLeftIconVM>(PropertyType.MainPageTopLeftIcon, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public ActionResult GetFooterSocialUrl()
        {
            return Json(PropertyService.GetBy<FooterSocialIconCreateUpdateVM>(PropertyType.FooterIcon, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public ActionResult GetOurCustomerList()
        {
            return Json(OurObjectService.GetListWeb(SiteSettingService.GetSiteSetting()?.Id, OurObjectType.OurCustomers));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
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

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult PushNotificationSubscribe([FromForm] ExternalNotificationServicePushSubscriptionCreateVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ExternalNotificationServicePushSubscription, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            ExternalNotificationServicePushSubscriptionService.Create(input, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ExternalNotificationServicePushSubscription, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(true);
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetCompanyTitle()
        {
            return Content(SiteSettingService.GetSiteSetting()?.Title);
        }

        [Route("S/{code}")]
        [HttpGet]
        public ActionResult ShortLink(string code)
        {
            var foundLink = ShortLinkService.GetBy(SiteSettingService.GetSiteSetting()?.Id, code);
            if (foundLink == null || string.IsNullOrWhiteSpace(foundLink.TargetLink))
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Redirect(foundLink.TargetLink);
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public async Task<ActionResult> LoginTest()
        {
            //return Json(await UserInquiry.GetUserInfo(SiteSettingService.GetSiteSetting()?.Id, "3501511663", "09904561385", "1986-12-08"));
            //return Json(await UserInquiry.GetDriverLicence(SiteSettingService.GetSiteSetting()?.Id, "3501511663", "09904561385"));
            return Json(await CarInquiry.CarDiscount(SiteSettingService.GetSiteSetting()?.Id, 44 + "", 11 + "", 5 + "", 416 + "", "0938674919"));
        }

        [Route("[Controller]/[Action]")]
        public ActionResult GetBaseInfo()
        {
            var foundSetting = SiteSettingService.GetSiteSetting();
            return Json((foundSetting != null ? new 
            {
                title = foundSetting.Title,
                logo = GlobalConfig.FileAccessHandlerUrl + foundSetting.Image96,
                tLogo = GlobalConfig.FileAccessHandlerUrl + foundSetting.ImageText
            } : new { }));
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public ActionResult UpdateTempDeleteMe()
        {
            UpdateAllSignatures.Update();
            return Json(true);
        }
    }
}
