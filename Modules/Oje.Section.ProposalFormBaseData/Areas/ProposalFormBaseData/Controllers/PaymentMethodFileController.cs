using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.ProposalFormBaseData.Areas.ProposalFormBaseData.Controllers
{
    [Area("ProposalFormBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Icon = "fa-file-image", Title = "فایل های شرایط پرداخت")]
    [CustomeAuthorizeFilter]
    public class PaymentMethodFileController: Controller
    {
        readonly IPaymentMethodFileService PaymentMethodFileService = null;
        readonly IPaymentMethodService PaymentMethodService = null;
        public PaymentMethodFileController(IPaymentMethodFileService PaymentMethodFileFileService, IPaymentMethodService PaymentMethodService)
        {
            this.PaymentMethodFileService = PaymentMethodFileFileService;
            this.PaymentMethodService = PaymentMethodService;
        }

        [AreaConfig(Title = "فایل های شرایط پرداخت", Icon = "fa-dollar-sign", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فایل های شرایط پرداخت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "PaymentMethodFile", new { area = "ProposalFormBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست فایل های شرایط پرداخت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormBaseData", "PaymentMethodFile")));
        }

        [AreaConfig(Title = "افزودن فایل های شرایط پرداخت جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdatePaymentMethodFileVM input)
        {
            return Json(PaymentMethodFileService.Create(input));
        }

        [AreaConfig(Title = "حذف فایل های شرایط پرداخت", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(PaymentMethodFileService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک فایل های شرایط پرداخت", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(PaymentMethodFileService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  فایل های شرایط پرداخت", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdatePaymentMethodFileVM input)
        {
            return Json(PaymentMethodFileService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست فایل های شرایط پرداخت", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] PaymentMethodFileMainGrid searchInput)
        {
            return Json(PaymentMethodFileService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] PaymentMethodFileMainGrid searchInput)
        {
            var result = PaymentMethodFileService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetPayMethodList()
        {
            return Json(PaymentMethodService.GetLightList());
        }
    }
}
