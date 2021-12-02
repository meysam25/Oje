using Microsoft.AspNetCore.Mvc;
using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Areas.InquiryBaseData.Controllers
{
    [Area("InquiryBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات استعلام", Icon = "fa-info", Title = "شرکت های استعلام")]
    [CustomeAuthorizeFilter]
    public class InquiryCompanyLimitController: Controller
    {
        readonly IInquiryCompanyLimitManager InquiryCompanyLimitManager = null;
        readonly ICompanyManager CompanyManager = null;
        public InquiryCompanyLimitController(ICompanyManager CompanyManager, IInquiryCompanyLimitManager InquiryCompanyLimitManager)
        {
            this.InquiryCompanyLimitManager = InquiryCompanyLimitManager;
            this.CompanyManager = CompanyManager;
        }

        [AreaConfig(Title = "شرکت های استعلام", Icon = "fa-building", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "شرکت های استعلام";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InquiryCompanyLimit", new { area = "InquiryBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست شرکت های استعلام", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InquiryBaseData", "InquiryCompanyLimit")));
        }

        [AreaConfig(Title = "افزودن شرکت های استعلام جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInquiryCompanyLimitVM input)
        {
            return Json(InquiryCompanyLimitManager.Create(input));
        }

        [AreaConfig(Title = "حذف شرکت های استعلام", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InquiryCompanyLimitManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک شرکت های استعلام", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InquiryCompanyLimitManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  شرکت های استعلام", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInquiryCompanyLimitVM input)
        {
            return Json(InquiryCompanyLimitManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های استعلام", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InquiryCompanyLimitMainGrid searchInput)
        {
            return Json(InquiryCompanyLimitManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InquiryCompanyLimitMainGrid searchInput)
        {
            var result = InquiryCompanyLimitManager.GetList(searchInput);
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
            return Json(CompanyManager.GetLightList());
        }
    }
}
