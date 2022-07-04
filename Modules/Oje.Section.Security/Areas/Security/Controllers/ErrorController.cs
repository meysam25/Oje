using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using System;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "لاگ خطا ها")]
    [CustomeAuthorizeFilter]
    public class ErrorController: Controller
    {
        readonly IErrorService ErrorService = null;
        public ErrorController(IErrorService ErrorService)
        {
            this.ErrorService = ErrorService;
        }

        [AreaConfig(Title = "لاگ خطا ها", Icon = "fa-bug", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لاگ خطا ها";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Error", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست لاگ خطا ها", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "Error")));
        }

        [AreaConfig(Title = "مشاهده لیست لاگ خطا ها", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ErrorMainGrid searchInput)
        {
            return Json(ErrorService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ErrorMainGrid searchInput)
        {
            var result = ErrorService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
