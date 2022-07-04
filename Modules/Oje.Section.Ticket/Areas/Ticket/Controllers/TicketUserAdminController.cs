using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Section.Ticket.Interfaces;
using Oje.Section.Ticket.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.Ticket.Areas.Ticket.Controllers
{
    [Area("Ticket")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تیکت", Icon = "fa-ticket", Title = "تیکت")]
    [CustomeAuthorizeFilter]
    public class TicketUserAdminController: Controller
    {
        readonly ITicketUserService TicketUserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ITicketCategoryService TicketCategoryService = null;
        readonly ITicketUserAnswerService TicketUserAnswerService = null;

        public TicketUserAdminController
            (
                ITicketUserService TicketUserService,
                ISiteSettingService SiteSettingService,
                ITicketCategoryService TicketCategoryService,
                ITicketUserAnswerService TicketUserAnswerService
            )
        {
            this.TicketUserService = TicketUserService;
            this.SiteSettingService = SiteSettingService;
            this.TicketCategoryService = TicketCategoryService;
            this.TicketUserAnswerService = TicketUserAnswerService;
        }

        [AreaConfig(Title = "تیکت", Icon = "fa-ticket", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تیکت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TicketUserAdmin", new { area = "Ticket" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تیکت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Ticket", "TicketUserAdmin")));
        }

        [AreaConfig(Title = "مشاهده لیست تیکت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] TicketUserAdminMainGrid searchInput)
        {
            return Json(TicketUserService.GetListForAdmin(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست پاسخ های", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetAnswers([FromForm] TicketUserAnswerMainGrid searchInput)
        {
            return Json(TicketUserAnswerService.GetListForAdmin(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "افزودن تیکت جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateAnswer([FromForm] TicketUserAnswerCreateUpdateVM input)
        {
            return Json(TicketUserAnswerService.CreateForAdmin(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] TicketUserAdminMainGrid searchInput)
        {
            var result = TicketUserService.GetListForAdmin(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

    }
}
