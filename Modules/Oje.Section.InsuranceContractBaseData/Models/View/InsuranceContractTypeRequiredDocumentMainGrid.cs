using Oje.Infrastructure.Models;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractTypeRequiredDocumentMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public int? cid { get; set; }
        public string ctId { get; set; }
        public bool? isRequired { get; set; }
        public bool? isActive { get; set; }
    }
}
