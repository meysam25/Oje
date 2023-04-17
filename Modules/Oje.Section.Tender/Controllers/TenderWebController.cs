using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Infrastructure;
using System.Collections.Generic;
using Oje.AccountService.Interfaces;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.View;

namespace Oje.Section.Tender.Controllers
{
    [Route("[Controller]/[Action]")]
    public class TenderWebController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly ITenderConfigService TenderConfigService = null;
        readonly ITenderFileService TenderFileService = null;
        readonly IPropertyService PropertyService = null;

        public TenderWebController
            (
                ISiteSettingService SiteSettingService,
                ITenderConfigService TenderConfigService,
                ITenderFileService TenderFileService,
                IPropertyService PropertyService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.TenderConfigService = TenderConfigService;
            this.TenderFileService = TenderFileService;
            this.PropertyService = PropertyService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var curSetting = SiteSettingService.GetSiteSetting();
            if (curSetting == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            ViewBag.tenderConfig = TenderConfigService.GetTitleAndSubTitleCache(curSetting.Id);

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
            return View();
        }

        [HttpGet]
        public IActionResult GetOurPrideMainPage()
        {
            return Json(PropertyService.GetBy<OurPrideVM>(PropertyType.OurPrideMainPageForTender, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        public IActionResult GetTenderFiles([FromForm] int? formId)
        {
            return Json(TenderFileService.GetListForWeb(SiteSettingService.GetSiteSetting()?.Id, formId));
        }

        [HttpPost]
        public IActionResult GetLoginConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Tender", "LoginPanel")));
        }

        [HttpPost]
        public IActionResult GetLoginForRegConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Tender", "LoginPanelForRegister")));
        }

        [HttpPost]
        public IActionResult GetSiteInfo()
        {
            return Json(new { title = SiteSettingService.GetSiteSetting()?.Title });
        }
    }
}
