using Oje.Infrastructure.Models;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractUserBaseInsuranceCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string code { get; set; }
        public string title { get; set; }
    }
}
