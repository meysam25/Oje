using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;

namespace Oje.Section.ProposalFilledForm.Areas.ProposalFilledForm.Controllers
{
    [Area("ProposalFilledForm")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت بیمه نامه ها", Icon = "fa-file-powerpoint", Title = "افزودن فرم پیشنهاد جدید")]
    [CustomeAuthorizeFilter]
    public class NewProposalFilledFormController : Controller
    {
        [AreaConfig(Title = "افزودن فرم پیشنهاد جدید", Icon = "fa-file-search", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Form()
        {
            ViewBag.Title = "افزودن فرم پیشنهاد جدید";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "NewProposalFilledForm", new { area = "ProposalFilledForm" });
            return View("Index");
        }

        [AreaConfig(Title = "تنظیمات افزودن فرم پیشنهاد جدید", Icon = "fa-cog")]
        [HttpPost]
        [HttpGet]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledForm", "NewProposalFilledForm")));
        }
    }
}
