using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.CarThirdBaseData.Areas.CarThirdBaseData.Controllers
{
    [Area("CarThirdBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو", Icon = "fa-car-side", Title = "تعهد مالی سال جاری")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyFinancialCommitmentController: Controller
    {
        readonly IThirdPartyFinancialCommitmentService ThirdPartyFinancialCommitmentService = null;
        public ThirdPartyFinancialCommitmentController(
                IThirdPartyFinancialCommitmentService ThirdPartyFinancialCommitmentService
            )
        {
            this.ThirdPartyFinancialCommitmentService = ThirdPartyFinancialCommitmentService;
        }

        [AreaConfig(Title = "تعهد مالی سال جاری", Icon = "fa-comments-dollar", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تعهد مالی سال جاری";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyFinancialCommitment", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تعهد مالی سال جاری", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyFinancialCommitment")));
        }

        [AreaConfig(Title = "افزودن تعهد مالی سال جاری جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyFinancialCommitmentVM input)
        {
            return Json(ThirdPartyFinancialCommitmentService.Create(input));
        }

        [AreaConfig(Title = "حذف تعهد مالی سال جاری", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyFinancialCommitmentService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تعهد مالی سال جاری", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyFinancialCommitmentService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تعهد مالی سال جاری", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyFinancialCommitmentVM input)
        {
            return Json(ThirdPartyFinancialCommitmentService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تعهد مالی سال جاری", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyFinancialCommitmentMainGrid searchInput)
        {
            return Json(ThirdPartyFinancialCommitmentService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyFinancialCommitmentMainGrid searchInput)
        {
            var result = ThirdPartyFinancialCommitmentService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
