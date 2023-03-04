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
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو", Icon = "fa-car-side", Title = "تعهد جانی سال جاری")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyLifeCommitmentController: Controller
    {
        readonly IThirdPartyLifeCommitmentService ThirdPartyLifeCommitmentService = null;
        public ThirdPartyLifeCommitmentController(
                IThirdPartyLifeCommitmentService ThirdPartyLifeCommitmentService
            )
        {
            this.ThirdPartyLifeCommitmentService = ThirdPartyLifeCommitmentService;
        }

        [AreaConfig(Title = "تعهد جانی سال جاری", Icon = "fa-comments-dollar", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تعهد جانی سال جاری";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyLifeCommitment", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تعهد جانی سال جاری", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyLifeCommitment")));
        }

        [AreaConfig(Title = "افزودن تعهد جانی سال جاری جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyLifeCommitmentVM input)
        {
            return Json(ThirdPartyLifeCommitmentService.Create(input));
        }

        [AreaConfig(Title = "حذف تعهد جانی سال جاری", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyLifeCommitmentService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تعهد جانی سال جاری", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyLifeCommitmentService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تعهد جانی سال جاری", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyLifeCommitmentVM input)
        {
            return Json(ThirdPartyLifeCommitmentService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تعهد جانی سال جاری", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyLifeCommitmentMainGrid searchInput)
        {
            return Json(ThirdPartyLifeCommitmentService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyLifeCommitmentMainGrid searchInput)
        {
            var result = ThirdPartyLifeCommitmentService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
