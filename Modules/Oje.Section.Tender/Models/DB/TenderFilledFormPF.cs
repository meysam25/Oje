using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("TenderFilledFormPFs")]
    public class TenderFilledFormPF
    {
        public long TenderFilledFormId { get; set; }
        [ForeignKey("TenderFilledFormId"), InverseProperty("TenderFilledFormPFs")]
        public TenderFilledForm TenderFilledForm { get; set; }
        public int TenderProposalFormJsonConfigId { get; set; }
        [ForeignKey("TenderProposalFormJsonConfigId"), InverseProperty("TenderFilledFormPFs")]
        public TenderProposalFormJsonConfig TenderProposalFormJsonConfig { get; set; }
        public bool? IsConfirmByAdmin { get; set; }
        public bool? IsConfirmByUser { get; set; }
    }
}
