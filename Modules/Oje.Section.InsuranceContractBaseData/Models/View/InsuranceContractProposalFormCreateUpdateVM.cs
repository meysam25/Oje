using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractProposalFormCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString jConfig { get; set; }
        public string description { get; set; }
        public bool? isActive { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString termsT { get; set; }
    }
}
