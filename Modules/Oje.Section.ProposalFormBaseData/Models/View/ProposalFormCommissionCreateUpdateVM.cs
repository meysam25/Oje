namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormCommissionCreateUpdateVM
    {
        public int? id { get; set; }
        public int? ppfId { get; set; }
        public string title { get; set; }
        public long? fPrice { get; set; }
        public long? tPrice { get; set; }
        public decimal? rate { get; set; }
    }
}
