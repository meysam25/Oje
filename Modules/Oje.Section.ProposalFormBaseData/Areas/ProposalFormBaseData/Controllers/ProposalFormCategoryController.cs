using Oje.AccountService.Filters;
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
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Icon = "fa-file-powerpoint", Title = "گروه بندی فرم پیشنهاد")]
    [CustomeAuthorizeFilter]
    public class ProposalFormCategoryController: Controller
    {
        readonly IProposalFormCategoryService ProposalFormCategoryService = null;
        public ProposalFormCategoryController(IProposalFormCategoryService ProposalFormCategoryService)
        {
            this.ProposalFormCategoryService = ProposalFormCategoryService;
        }

        [AreaConfig(Title = "گروه بندی فرم پیشنهاد", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گروه بندی فرم پیشنهاد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFormCategory", new { area = "ProposalFormBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست گروه بندی فرم پیشنهاد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormBaseData", "ProposalFormCategory")));
        }

        [AreaConfig(Title = "افزودن گروه بندی فرم پیشنهاد جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateProposalFormCategoryVM input)
        {
            return Json(ProposalFormCategoryService.Create(input));
        }

        [AreaConfig(Title = "حذف گروه بندی فرم پیشنهاد", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormCategoryService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک گروه بندی فرم پیشنهاد", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormCategoryService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  گروه بندی فرم پیشنهاد", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateProposalFormCategoryVM input)
        {
            return Json(ProposalFormCategoryService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی فرم پیشنهاد", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFormCategoryMainGrid searchInput)
        {
            return Json(ProposalFormCategoryService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFormCategoryMainGrid searchInput)
        {
            var result = ProposalFormCategoryService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
