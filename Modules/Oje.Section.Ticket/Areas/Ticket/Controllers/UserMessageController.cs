using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.Ticket.Areas.Ticket.Controllers
{
    [Area("Ticket")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پشتیبانی تیکت و پیام ها", Order = 9, Icon = "fa-ticket", Title = "پیام")]
    [CustomeAuthorizeFilter]
    public class UserMessageController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUserMessageService UserMessageService = null;
        readonly IUserService UserService = null;
        readonly IUserMessageReplyService UserMessageReplyService = null;

        public UserMessageController
            (
                ISiteSettingService SiteSettingService,
                IUserMessageService UserMessageService,
                IUserService UserService,
                IUserMessageReplyService UserMessageReplyService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.UserMessageService = UserMessageService;
            this.UserService = UserService;
            this.UserMessageReplyService = UserMessageReplyService;
        }

        [AreaConfig(Title = "پیام", Icon = "fa-comment", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "پیام";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserMessage", new { area = "Ticket" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست پیام", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Ticket", "UserMessage")));
        }

        [AreaConfig(Title = "افزودن پیام جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] UserMessageCreateVM input)
        {
            return Json(UserMessageService.Create(input, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست پیام", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserMessageMainGrid searchInput)
        {
            return Json(UserMessageService.GetList(searchInput, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserMessageMainGrid searchInput)
        {
            var result = UserMessageService.GetList(searchInput, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "افزودن پاسخ جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateReply([FromForm] UserMessageCreateVM input)
        {
            return Json(UserMessageReplyService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست پاسخ ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetReplyList([FromForm] GlobalGridParentLong searchInput)
        {
            return Json(UserMessageReplyService.GetList(searchInput,SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetUserList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(UserService.GetSelect2List(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
