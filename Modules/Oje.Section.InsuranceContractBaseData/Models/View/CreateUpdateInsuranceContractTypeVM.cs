using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class CreateUpdateInsuranceContractTypeVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString desc { get; set; }
    }
}
