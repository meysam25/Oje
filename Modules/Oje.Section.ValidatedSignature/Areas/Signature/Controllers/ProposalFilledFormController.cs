using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using System;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;
using Oje.Security.Services;

namespace Oje.Section.ValidatedSignature.Areas.Signature.Controllers
{
    [Area("Signature")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "اعتبارسنجی امضا", Icon = "fa-spider-black-widow", Title = "فرم پیشنهاد")]
    [CustomeAuthorizeFilter]
    public class ProposalFilledFormController: Controller
    {
        readonly IProposalFilledFormService ProposalFilledFormService = null;

        public ProposalFilledFormController
            (
                IProposalFilledFormService ProposalFilledFormService
            )
        {
            this.ProposalFilledFormService = ProposalFilledFormService;
        }

        [AreaConfig(Title = "فرم پیشنهاد", Icon = "fa-check-circle", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فرم پیشنهاد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFilledForm", new { area = "Signature" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست فرم پیشنهاد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Signature", "ProposalFilledForm")));
        }

        [AreaConfig(Title = "مشاهده جزئیات تغییرات غیر معتبر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] long? id)
        {
            return Json(ProposalFilledFormService.GetBy(id));
        }

        [AreaConfig(Title = "مشاهده لیست فرم پیشنهاد", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFilledFormMainGrid searchInput)
        {
            return Json(ProposalFilledFormService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFilledFormMainGrid searchInput)
        {
            var result = ProposalFilledFormService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
