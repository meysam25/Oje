using Oje.Infrastructure.Models;

namespace Oje.ValidatedSignature.Models.View
{
    public class ProposalFilledFormMainGrid: GlobalGrid
    {
        public long? id { get; set; }
        public string companyTitle { get; set; }
        public string formTitle { get; set; }
        public string createDate { get; set; }
        public string website { get; set; }
    }
}
