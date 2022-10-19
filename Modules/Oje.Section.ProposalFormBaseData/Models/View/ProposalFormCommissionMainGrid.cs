using Oje.Infrastructure.Models;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormCommissionMainGrid: GlobalGrid
    {
        public string ppf { get; set; }
        public string title { get; set; }
        public long? fPrice { get; set; }
        public long? tPrice { get; set; }
        public decimal? rate { get; set; }
    }
}
