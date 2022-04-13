using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Ticket.Interfaces;
using Oje.Section.Ticket.Models.View;
using System;

namespace Oje.Section.Ticket.Areas.Ticket.Controllers
{
    [Area("Ticket")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تیکت", Icon = "fa-ticket", Title = "گروه بندی")]
    [CustomeAuthorizeFilter]
    public class TicketCategoryController: Controller
    {
        readonly ITicketCategoryService TicketCategoryService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public TicketCategoryController
            (
                ITicketCategoryService TicketCategoryService,
                ISiteSettingService SiteSettingService
            )
        {
            this.TicketCategoryService = TicketCategoryService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "گروه بندی", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گروه بندی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TicketCategory", new { area = "Ticket" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست گروه بندی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Ticket", "TicketCategory")));
        }

        [AreaConfig(Title = "افزودن گروه بندی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] TicketCategoryCreateUpdateVM input)
        {
            return Json(TicketCategoryService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف گروه بندی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(TicketCategoryService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک گروه بندی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(TicketCategoryService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  گروه بندی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] TicketCategoryCreateUpdateVM input)
        {
            return Json(TicketCategoryService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] TicketCategoryMainGrid searchInput)
        {
            return Json(TicketCategoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] TicketCategoryMainGrid searchInput)
        {
            var result = TicketCategoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
