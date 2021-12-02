using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseData.Controllers
{
    [Area("InsuranceContractBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "لیست بیمه شدگان دائم")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractUserPremanentController: Controller
    {
        readonly IInsuranceContractUserManager InsuranceContractUserManager = null;
        readonly IInsuranceContractManager InsuranceContractManager = null;
        public InsuranceContractUserPremanentController(
                IInsuranceContractUserManager InsuranceContractUserManager,
                IInsuranceContractManager InsuranceContractManager
            )
        {
            this.InsuranceContractUserManager = InsuranceContractUserManager;
            this.InsuranceContractManager = InsuranceContractManager;
        }

        [AreaConfig(Title = "لیست بیمه شدگان دائم", Icon = "fa-user-check", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست بیمه شدگان دائم";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractUserPremanent", new { area = "InsuranceContractBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست بیمه شدگان دائم", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "InsuranceContractUserPremanent")));
        }

        [AreaConfig(Title = "تایید بیمه شدگان دائم", Icon = "fa-check")]
        [HttpPost]
        public IActionResult Confirm([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractUserManager.ChangeStatus(input?.id, InsuranceContractUserStatus.Premanent, InsuranceContractUserStatus.Temprory));
        }

        [AreaConfig(Title = "افزودن لیست بیمه شدگان دائم جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInsuranceContractUserVM input)
        {
            return Json(InsuranceContractUserManager.Create(input, InsuranceContractUserStatus.Premanent));
        }

        [AreaConfig(Title = "افزودن بیمه شدگان دائم از روی فایل اکسل", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateFromXcel([FromForm] GlobalExcelFile input)
        {
            return Json(InsuranceContractUserManager.CreateFromExcel(input, InsuranceContractUserStatus.Premanent));
        }

        [AreaConfig(Title = "حذف بیمه شدگان دائم", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractUserManager.Delete(input?.id, InsuranceContractUserStatus.Premanent));
        }

        [AreaConfig(Title = "مشاهده یک  بیمه شدگان دائم", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractUserManager.GetById(input?.id, InsuranceContractUserStatus.Premanent));
        }

        [AreaConfig(Title = "به روز رسانی بیمه شدگان دائم", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInsuranceContractUserVM input)
        {
            return Json(InsuranceContractUserManager.Update(input, InsuranceContractUserStatus.Premanent));
        }

        [AreaConfig(Title = "مشاهده لیست بیمه شدگان دائم", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractUserMainGrid searchInput)
        {
            return Json(InsuranceContractUserManager.GetList(searchInput, InsuranceContractUserStatus.Premanent));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractUserMainGrid searchInput)
        {
            var result = InsuranceContractUserManager.GetList(searchInput, InsuranceContractUserStatus.Premanent);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست قرارداد", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetContractList()
        {
            return Json(InsuranceContractManager.GetLightList());
        }
    }
}
