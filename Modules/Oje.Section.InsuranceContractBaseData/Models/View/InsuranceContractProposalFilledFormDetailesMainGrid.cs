using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractProposalFilledFormDetailesMainGrid : GlobalGridParentLong
    {
        public string userFullname { get; set; }
        public string type { get; set; }
        public string birthDate { get; set; }
        public long? price { get; set; }
        public InsuranceContractUserFamilyRelation? relation { get; set; }
    }
}
