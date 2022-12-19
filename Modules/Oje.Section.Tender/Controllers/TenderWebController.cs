using Microsoft.AspNetCore.Mvc;

namespace Oje.Section.Tender.Controllers
{
    [Route("[Controller]/[Action]")]
    public class TenderWebController: Controller
    {
        public TenderWebController()
        {

        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
