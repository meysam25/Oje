using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.SalesNetworkBaseData.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Section.SalesNetworkBaseData.Models.View;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.SalesNetworkBaseData.Areas.SalesNetworkBaseData.Controllers
{
    [Area("SalesNetworkBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات شبکه فروش/بازاریاب", Icon = "fa-network-wired", Title = "شبکه فروش")]
    [CustomeAuthorizeFilter]
    public class SalesNetworkController: Controller
    {
        readonly ISalesNetworkService SalesNetworkService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly ICompanyService CompanyService = null;
        readonly AccountService.Interfaces.IUserService UserService = null;
        public SalesNetworkController(
                ISalesNetworkService SalesNetworkService,
                IProposalFormService ProposalFormService,
                ICompanyService CompanyService,
                AccountService.Interfaces.IUserService UserService
            )
        {
            this.SalesNetworkService = SalesNetworkService;
            this.ProposalFormService = ProposalFormService;
            this.CompanyService = CompanyService;
            this.UserService = UserService;
        }

        [AreaConfig(Title = "شبکه فروش", Icon = "fa-project-diagram", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "شبکه فروش";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SalesNetwork", new { area = "SalesNetworkBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست شبکه فروش", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SalesNetworkBaseData", "SalesNetwork")));
        }

        [AreaConfig(Title = "افزودن شبکه فروش جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateSalesNetworkVM input)
        {
            return Json(SalesNetworkService.Create(input));
        }

        [AreaConfig(Title = "حذف شبکه فروش", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SalesNetworkService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک شبکه فروش", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SalesNetworkService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  شبکه فروش", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateSalesNetworkVM input)
        {
            return Json(SalesNetworkService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست شبکه فروش", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SalesNetworkMainGrid searchInput)
        {
            return Json(SalesNetworkService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SalesNetworkMainGrid searchInput)
        {
            var result = SalesNetworkService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست بازاریاب", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetMarketerList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(UserService.GetSelect2ListByType(searchInput, RoleType.Marketer));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }
    }
}
