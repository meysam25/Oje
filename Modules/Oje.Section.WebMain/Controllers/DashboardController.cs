using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ViewBag.Title = "داشبورد " + HttpContext.GetLoginUser().Fullname;
            return View();
        }
    }
}
