using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Interfaces;
using Oje.Section.BaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.BaseData.Areas.BaseData.Controllers
{
    [Area("BaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه", Icon = "fa-archive", Title = "گروه خطر مشاغل")]
    [CustomeAuthorizeFilter]
    public class JobDangerLevelController : Controller
    {
        readonly IJobDangerLevelService JobDangerLevelService = null;

        public JobDangerLevelController(IJobDangerLevelService JobDangerLevelService)
        {
            this.JobDangerLevelService = JobDangerLevelService;
        }

        [AreaConfig(Title = "گروه خطر مشاغل", Icon = "fa-biohazard", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گروه خطر مشاغل";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "JobDangerLevel", new { area = "BaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست گروه خطر مشاغل", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BaseData", "JobDangerLevel")));
        }

        [AreaConfig(Title = "افزودن گروه خطر مشاغل جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateJobDangerLevelVM input)
        {
            return Json(JobDangerLevelService.Create(input));
        }

        [AreaConfig(Title = "حذف گروه خطر مشاغل", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(JobDangerLevelService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک گروه خطر مشاغل", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(JobDangerLevelService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  گروه خطر مشاغل", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateJobDangerLevelVM input)
        {
            return Json(JobDangerLevelService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست گروه خطر مشاغل", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] JobDangerLevelMainGrid searchInput)
        {
            return Json(JobDangerLevelService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] JobDangerLevelMainGrid searchInput)
        {
            var result = JobDangerLevelService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
