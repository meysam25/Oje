using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.View;
using System;

namespace Oje.Section.ProposalFormBaseData.Areas.ProposalFormBaseData.Controllers
{
    [Area("ProposalFormBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Icon = "fa-file-powerpoint", Title = "شرایط پرداخت")]
    [CustomeAuthorizeFilter]
    public class PaymentMethodController: Controller
    {
        readonly IPaymentMethodService PaymentMethodService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly ICompanyService CompanyService = null;
        public PaymentMethodController(IPaymentMethodService PaymentMethodService, IProposalFormService ProposalFormService, ICompanyService CompanyService)
        {
            this.PaymentMethodService = PaymentMethodService;
            this.ProposalFormService = ProposalFormService;
            this.CompanyService = CompanyService;
        }

        [AreaConfig(Title = "شرایط پرداخت", Icon = "fa-dollar-sign", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "شرایط پرداخت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "PaymentMethod", new { area = "ProposalFormBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست شرایط پرداخت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormBaseData", "PaymentMethod")));
        }

        [AreaConfig(Title = "افزودن شرایط پرداخت جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdatePaymentMethodVM input)
        {
            return Json(PaymentMethodService.Create(input));
        }

        [AreaConfig(Title = "حذف شرایط پرداخت", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(PaymentMethodService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک شرایط پرداخت", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(PaymentMethodService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  شرایط پرداخت", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdatePaymentMethodVM input)
        {
            return Json(PaymentMethodService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست شرایط پرداخت", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] PaymentMethodMainGrid searchInput)
        {
            return Json(PaymentMethodService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] PaymentMethodMainGrid searchInput)
        {
            var result = PaymentMethodService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList(HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput));
        }
    }
}
