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
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو", Icon = "fa-car-side", Title = "تعهد مالی درخواستی (پوشش های مالی)")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyRequiredFinancialCommitmentController: Controller
    {
        readonly IThirdPartyRequiredFinancialCommitmentService ThirdPartyRequiredFinancialCommitmentService = null;
        readonly ICompanyService CompanyService = null;
        public ThirdPartyRequiredFinancialCommitmentController
            (
                IThirdPartyRequiredFinancialCommitmentService ThirdPartyRequiredFinancialCommitmentService,
                ICompanyService CompanyService
            )
        {
            this.ThirdPartyRequiredFinancialCommitmentService = ThirdPartyRequiredFinancialCommitmentService;
            this.CompanyService = CompanyService;
        }

        [AreaConfig(Title = "تعهد مالی درخواستی (پوشش های مالی)", Icon = "fa-comments-dollar", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تعهد مالی درخواستی (پوشش های مالی)";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyRequiredFinancialCommitment", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تعهد مالی درخواستی (پوشش های مالی)", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyRequiredFinancialCommitment")));
        }

        [AreaConfig(Title = "افزودن تعهد مالی درخواستی (پوشش های مالی) جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyRequiredFinancialCommitmentVM input)
        {
            return Json(ThirdPartyRequiredFinancialCommitmentService.Create(input));
        }

        [AreaConfig(Title = "حذف تعهد مالی درخواستی (پوشش های مالی)", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyRequiredFinancialCommitmentService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تعهد مالی درخواستی (پوشش های مالی)", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyRequiredFinancialCommitmentService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تعهد مالی درخواستی (پوشش های مالی)", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyRequiredFinancialCommitmentVM input)
        {
            return Json(ThirdPartyRequiredFinancialCommitmentService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تعهد مالی درخواستی (پوشش های مالی)", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyRequiredFinancialCommitmentMainGrid searchInput)
        {
            return Json(ThirdPartyRequiredFinancialCommitmentService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyRequiredFinancialCommitmentMainGrid searchInput)
        {
            var result = ThirdPartyRequiredFinancialCommitmentService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }
    }
}
