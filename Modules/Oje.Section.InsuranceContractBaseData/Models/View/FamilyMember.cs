using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class FamilyMember
    {
        public FamilyMember()
        {
            files = new();
        }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string typeTitle { get; set; }
        public string relation { get; set; }
        public string desc { get; set; }

        public List<FamilyMemberFile> files { get; set;}
    }
}
