using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Ticket.Interfaces;
using Oje.Section.Ticket.Models.View;
using Oje.Security.Interfaces;

namespace Oje.Section.Ticket.Areas.Ticket.Controllers
{
    [Area("Ticket")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پشتیبانی تیکت و پیام ها", Order = 9, Icon = "fa-ticket", Title = "تیکت های من")]
    [CustomeAuthorizeFilter]
    public class TicketUserController : Controller
    {
        readonly ITicketUserService TicketUserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ITicketCategoryService TicketCategoryService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly ITicketUserAnswerService TicketUserAnswerService = null;

        public TicketUserController
            (
                ITicketUserService TicketUserService,
                ISiteSettingService SiteSettingService,
                ITicketCategoryService TicketCategoryService,
                IBlockAutoIpService BlockAutoIpService,
                ITicketUserAnswerService TicketUserAnswerService
            )
        {
            this.TicketUserService = TicketUserService;
            this.SiteSettingService = SiteSettingService;
            this.TicketCategoryService = TicketCategoryService;
            this.BlockAutoIpService = BlockAutoIpService;
            this.TicketUserAnswerService = TicketUserAnswerService;
        }

        [AreaConfig(Title = "تیکت های من", Icon = "fa-ticket", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تیکت های من";
            ViewBag.layer = "_WebLayout";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TicketUser", new { area = "Ticket" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تیکت های من", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Ticket", "TicketUser")));
        }

        [AreaConfig(Title = "افزودن تیکت های من جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] TicketUserCreateUpdateVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateNewTicket, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = TicketUserService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateNewTicket, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [AreaConfig(Title = "مشاهده لیست تیکت های من", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] TicketUserMainGrid searchInput)
        {
            return Json(TicketUserService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست پاسخ های من", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetAnswers([FromForm] TicketUserAnswerMainGrid searchInput)
        {
            return Json(TicketUserAnswerService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "پاسخ دادن تیکت های من جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateAnswer([FromForm] TicketUserAnswerCreateUpdateVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateNewAnswerForTicket, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = TicketUserAnswerService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateNewAnswerForTicket, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی تیکت های من", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCatList()
        {
            return Json(TicketCategoryService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست زیر گروه گروه بندی تیکت های من", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetSubCatList([FromQuery] Select2SearchVM searchInput, [FromQuery] int catId)
        {
            return Json(TicketCategoryService.GetightListForSelect2(catId, searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
