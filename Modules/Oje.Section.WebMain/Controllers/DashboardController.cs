using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure.Services;

namespace Oje.Section.WebMain.Controllers
{
    public class DashboardController: Controller
    {
        public DashboardController()
        {

        }

        [Route("Dashboard")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "داشبورد " + HttpContext.GetLoginUser()?.Fullname;
            return View();
        }
    }
}
