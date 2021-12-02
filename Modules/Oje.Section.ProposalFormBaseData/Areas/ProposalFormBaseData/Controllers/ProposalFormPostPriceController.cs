using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Areas.ProposalFormBaseData.Controllers
{
    [Area("ProposalFormBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Icon = "fa-file-powerpoint", Title = "هزینه پست")]
    [CustomeAuthorizeFilter]
    public class ProposalFormPostPriceController : Controller
    {
        readonly IProposalFormPostPriceManager ProposalFormPostPriceManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        public ProposalFormPostPriceController(
                IProposalFormPostPriceManager ProposalFormPostPriceManager,
                IProposalFormManager ProposalFormManager
            )
        {
            this.ProposalFormPostPriceManager = ProposalFormPostPriceManager;
            this.ProposalFormManager = ProposalFormManager;
        }

        [AreaConfig(Title = "هزینه پست", Icon = "fa-mail-bulk", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "هزینه پست";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFormPostPrice", new { area = "ProposalFormBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست هزینه پست", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormBaseData", "ProposalFormPostPrice")));
        }

        [AreaConfig(Title = "افزودن هزینه پست جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateProposalFormPostPriceVM input)
        {
            return Json(ProposalFormPostPriceManager.Create(input));
        }

        [AreaConfig(Title = "حذف هزینه پست", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormPostPriceManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک هزینه پست", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormPostPriceManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  هزینه پست", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateProposalFormPostPriceVM input)
        {
            return Json(ProposalFormPostPriceManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست هزینه پست", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFormPostPriceMainGrid searchInput)
        {
            return Json(ProposalFormPostPriceManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFormPostPriceMainGrid searchInput)
        {
            var result = ProposalFormPostPriceManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormManager.GetSelect2List(searchInput));
        }
    }
}
