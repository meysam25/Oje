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
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو (ادمین)", Icon = "fa-car-side", Title = "تعهد مالی راننده سال جاری")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyDriverFinancialCommitmentController: Controller
    {
        readonly IThirdPartyDriverFinancialCommitmentManager ThirdPartyDriverFinancialCommitmentManager = null;
        public ThirdPartyDriverFinancialCommitmentController(IThirdPartyDriverFinancialCommitmentManager ThirdPartyDriverFinancialCommitmentManager)
        {
            this.ThirdPartyDriverFinancialCommitmentManager = ThirdPartyDriverFinancialCommitmentManager;
        }

        [AreaConfig(Title = "تعهد مالی راننده سال جاری", Icon = "fa-comments-dollar", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تعهد مالی راننده سال جاری";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyDriverFinancialCommitment", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تعهد مالی راننده سال جاری", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyDriverFinancialCommitment")));
        }

        [AreaConfig(Title = "افزودن تعهد مالی راننده سال جاری جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyDriverFinancialCommitmentVM input)
        {
            return Json(ThirdPartyDriverFinancialCommitmentManager.Create(input));
        }

        [AreaConfig(Title = "حذف تعهد مالی راننده سال جاری", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyDriverFinancialCommitmentManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تعهد مالی راننده سال جاری", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyDriverFinancialCommitmentManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تعهد مالی راننده سال جاری", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyDriverFinancialCommitmentVM input)
        {
            return Json(ThirdPartyDriverFinancialCommitmentManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تعهد مالی راننده سال جاری", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyDriverFinancialCommitmentMainGrid searchInput)
        {
            return Json(ThirdPartyDriverFinancialCommitmentManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyDriverFinancialCommitmentMainGrid searchInput)
        {
            var result = ThirdPartyDriverFinancialCommitmentManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
