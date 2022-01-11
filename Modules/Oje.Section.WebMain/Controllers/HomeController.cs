using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Areas.WebMain.Controllers
{
    public class HomeController: Controller
    {
        [Route("/")]
        [Route("[Controller]/[Action]")]
        [HttpGet]
        public IActionResult Index ()
        {
            ViewBag.Title = "ستاد بیمه";
            return View();
        }

        [Route("[Controller]/[Action]")]
        public IActionResult GetLoginModalConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMain", "LoginModal")));
        }
    }
}
