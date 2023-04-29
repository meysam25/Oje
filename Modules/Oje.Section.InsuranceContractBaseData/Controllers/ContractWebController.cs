using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Controllers
{
    [Route("[Controller]/[Action]")]
    public class ContractWebController: Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ITreatmentMainSliderService TreatmentMainSliderService = null;
        public ContractWebController(
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService,
                ITreatmentMainSliderService TreatmentMainSliderService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
            this.TreatmentMainSliderService = TreatmentMainSliderService;
        }

        [HttpGet]
        public ActionResult Index()
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
            return View();
        }

        [HttpPost]
        public ActionResult GetMainDescription()
        {
            return Json(PropertyService.GetBy<IndexPageDescriptionVM>(PropertyType.TreatmentIndexPageDescription, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpGet]
        public ActionResult GetSlider()
        {
            return Json(TreatmentMainSliderService.GetListFormWebsite(SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpGet]
        public IActionResult GetLoginConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseDataWeb", "LoginPanel")));
        }
    }
}
