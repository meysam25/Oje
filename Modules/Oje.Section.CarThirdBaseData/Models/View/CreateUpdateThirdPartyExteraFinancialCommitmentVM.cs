using System.Collections.Generic;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class CreateUpdateThirdPartyExteraFinancialCommitmentVM
    {

        public int? id { get; set; }
        public List<int> cIds { get; set; }
        public string title { get; set; }
        public int? carSpecId { get; set; }
        public int? thirdPartyRequiredFinancialCommitmentId { get; set; }
        public int year { get; set; }
        public decimal? rate { get; set; }
        public bool? isActive { get; set; }

        public string carSpecId_Title { get; set; }
        public string thirdPartyRequiredFinancialCommitmentId_Title { get; set; }


    }
}
