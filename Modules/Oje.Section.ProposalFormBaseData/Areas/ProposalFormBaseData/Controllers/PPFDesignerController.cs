using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;

namespace Oje.Section.ProposalFormBaseData.Areas.ProposalFormBaseData.Controllers
{
    [Area("ProposalFormBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Icon = "fa-file-powerpoint", Title = "طراحی فرم پیشنهاد")]
    [CustomeAuthorizeFilter]
    public class PPFDesignerController: Controller
    {
        public PPFDesignerController()
        {

        }

        [AreaConfig(Title = "طراحی فرم پیشنهاد", Icon = "fa-palette", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "طراحی فرم پیشنهاد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "PPFDesigner", new { area = "ProposalFormBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه طراحی فرم پیشنهاد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormBaseData", "PPFDesigner")));
        }
    }
}
