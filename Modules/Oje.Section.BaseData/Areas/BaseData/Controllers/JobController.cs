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
    [AreaConfig(ModualTitle = "پایه", Icon = "fa-archive", Title = "شغل")]
    [CustomeAuthorizeFilter]
    public class JobController: Controller
    {
        readonly IJobService JobService = null;
        readonly IJobDangerLevelService JobDangerLevelService = null;
        public JobController(IJobService JobService, IJobDangerLevelService JobDangerLevelService)
        {
            this.JobService = JobService;
            this.JobDangerLevelService = JobDangerLevelService;
        }

        [AreaConfig(Title = "مشاغل", Icon = "fa-user-md", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مشاغل";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Job", new { area = "BaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مشاغل", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BaseData", "Job")));
        }

        [AreaConfig(Title = "افزودن مشاغل جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateJobVM input)
        {
            return Json(JobService.Create(input));
        }

        [AreaConfig(Title = "حذف مشاغل", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(JobService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک مشاغل", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(JobService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  مشاغل", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateJobVM input)
        {
            return Json(JobService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست مشاغل", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] JobMainGrid searchInput)
        {
            return Json(JobService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] JobMainGrid searchInput)
        {
            var result = JobService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست استان", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetDangerLevel()
        {
            return Json(JobDangerLevelService.GetLightList());
        }
    }
}
