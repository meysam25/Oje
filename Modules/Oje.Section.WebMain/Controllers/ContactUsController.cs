using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.View;
using Oje.Security.Interfaces;

namespace Oje.Section.WebMain.Controllers
{
    public class ContactUsController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IPropertyService PropertyService = null;
        readonly IContactUsService ContactUsService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        public ContactUsController(
            ISiteSettingService SiteSettingService,
            IPropertyService PropertyService,
            IContactUsService ContactUsService,
            IBlockAutoIpService BlockAutoIpService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.PropertyService = PropertyService;
            this.ContactUsService = ContactUsService;
            this.BlockAutoIpService = BlockAutoIpService;
        }

        [Route("ContactUs")]
        [HttpGet]
        public ActionResult ContactUs()
        {
            var foundDescription = PropertyService.GetBy<ContactUsVM>(PropertyType.ContactUs, SiteSettingService.GetSiteSetting()?.Id);
            if (foundDescription != null)
                GlobalServices.FillSeoInfo(
                     ViewData,
                      foundDescription.title,
                      foundDescription.description,
                      Request.Scheme + "://" + Request.Host + "/",
                      Request.Scheme + "://" + Request.Host + "/",
                      WebSiteTypes.website,
                      Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                      null
                      );
            ViewBag.subTitle = foundDescription?.subTitle;
            return View();
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult Get()
        {
            var tempResult = PropertyService.GetBy<ContactUsVM>(PropertyType.ContactUs, SiteSettingService.GetSiteSetting()?.Id);
            return Json(new 
            {
                mapLat = tempResult?.mapLat,
                mapLon = tempResult?.mapLon,
                mapZoom = tempResult?.mapZoom
            });
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public ActionResult Create(ContactUsWebVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ContactUs, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = ContactUsService.Create(SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetIpAddress(), input);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ContactUs, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMain", "ContactUs")));
        }
    }
}
