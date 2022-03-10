using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public HomeController
            (
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService,
                IUserService UserService,
                IFooterExteraLinkService FooterExteraLinkService,
                IFooterGroupExteraLinkService FooterGroupExteraLinkService,
                ProposalFormService.Interfaces.IProposalFormCategoryService ProposalFormCategoryService,
                ProposalFormService.Interfaces.IProposalFormService ProposalFormService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
            this.UserService = UserService;
            this.FooterExteraLinkService = FooterExteraLinkService;
            this.FooterGroupExteraLinkService = FooterGroupExteraLinkService;
            this.ProposalFormCategoryService = ProposalFormCategoryService;
            this.ProposalFormService = ProposalFormService;
        }

        [Route("/")]
        [Route("[Controller]/[Action]")]
        [HttpGet]
        public IActionResult Index()
        {
            var curSetting = SiteSettingService.GetSiteSetting();
            if (curSetting == null)
                return NotFound();

            ViewBag.Title = curSetting.Title;

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
    }
}
