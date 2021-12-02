using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Areas.CarThirdBaseData.Controllers
{
    [Area("CarThirdBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو (ادمین)", Icon = "fa-car-side", Title = "درصد حق بیمه سال ساخت ثالث")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyCarCreateDatePercentController: Controller
    {
        readonly IThirdPartyCarCreateDatePercentManager ThirdPartyCarCreateDatePercentManager = null;
        public ThirdPartyCarCreateDatePercentController(IThirdPartyCarCreateDatePercentManager ThirdPartyCarCreateDatePercentManager)
        {
            this.ThirdPartyCarCreateDatePercentManager = ThirdPartyCarCreateDatePercentManager;
        }

        [AreaConfig(Title = "درصد حق بیمه سال ساخت ثالث", Icon = "fa-comments-dollar", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "درصد حق بیمه سال ساخت ثالث";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyCarCreateDatePercent", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست درصد حق بیمه سال ساخت ثالث", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyCarCreateDatePercent")));
        }

        [AreaConfig(Title = "افزودن درصد حق بیمه سال ساخت ثالث جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyCarCreateDatePercentVM input)
        {
            return Json(ThirdPartyCarCreateDatePercentManager.Create(input));
        }

        [AreaConfig(Title = "حذف درصد حق بیمه سال ساخت ثالث", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyCarCreateDatePercentManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک درصد حق بیمه سال ساخت ثالث", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyCarCreateDatePercentManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  درصد حق بیمه سال ساخت ثالث", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyCarCreateDatePercentVM input)
        {
            return Json(ThirdPartyCarCreateDatePercentManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست درصد حق بیمه سال ساخت ثالث", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyCarCreateDatePercentMainGrid searchInput)
        {
            return Json(ThirdPartyCarCreateDatePercentManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyCarCreateDatePercentMainGrid searchInput)
        {
            var result = ThirdPartyCarCreateDatePercentManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
