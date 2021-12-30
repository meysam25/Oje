using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index ()
        {
            ViewBag.Title = "ستاد بیمه";
            return View();
        }
    }
}
