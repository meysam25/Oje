using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Section.InsuranceContractBaseData.Models.View;

namespace Oje.Section.InsuranceContractBaseData.Controllers
{
    [Route("[Controller]/[Action]")]
    public class ContractWebController: Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public ContractWebController(
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetMainDescription()
        {
            return Json(PropertyService.GetBy<IndexPageDescriptionVM>(PropertyType.TreatmentIndexPageDescription, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
