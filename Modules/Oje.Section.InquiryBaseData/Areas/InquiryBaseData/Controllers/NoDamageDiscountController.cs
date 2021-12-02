using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Areas.BaseData.Controllers
{
    [Area("InquiryBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات استعلام", Icon = "fa-info", Title = "تخفیف عدم خسارت")]
    [CustomeAuthorizeFilter]
    public class NoDamageDiscountController: Controller
    {
        readonly INoDamageDiscountManager NoDamageDiscountManager = null;
        readonly ICompanyManager CompanyManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        public NoDamageDiscountController(
                INoDamageDiscountManager NoDamageDiscountManager, ICompanyManager CompanyManager, IProposalFormManager ProposalFormManager
            )
        {
            this.NoDamageDiscountManager = NoDamageDiscountManager;
            this.CompanyManager = CompanyManager;
            this.ProposalFormManager = ProposalFormManager;
        }

        [AreaConfig(Title = "تخفیف عدم خسارت", Icon = "fa-percent", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تخفیف عدم خسارت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "NoDamageDiscount", new { area = "InquiryBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تخفیف عدم خسارت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InquiryBaseData", "NoDamageDiscount")));
        }

        [AreaConfig(Title = "افزودن تخفیف عدم خسارت جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateNoDamageDiscountVM input)
        {
            return Json(NoDamageDiscountManager.Create(input));
        }

        [AreaConfig(Title = "حذف تخفیف عدم خسارت", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(NoDamageDiscountManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تخفیف عدم خسارت", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(NoDamageDiscountManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تخفیف عدم خسارت", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateNoDamageDiscountVM input)
        {
            return Json(NoDamageDiscountManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تخفیف عدم خسارت", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] NoDamageDiscountMainGrid searchInput)
        {
            return Json(NoDamageDiscountManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] NoDamageDiscountMainGrid searchInput)
        {
            var result = NoDamageDiscountManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های بیمه", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های بیمه", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetPPFList(Select2SearchVM searchInput)
        {
            return Json(ProposalFormManager.GetSelect2List(searchInput));
        }
    }
}
