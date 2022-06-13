using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("TenderFilledFormsValues")]
    public class TenderFilledFormsValue
    {
        [Key]
        public Guid Id { get; set; }
        public long TenderFilledFormId { get; set; }
        [ForeignKey("TenderFilledFormId"), InverseProperty("TenderFilledFormsValues")]
        public TenderFilledForm TenderFilledForm { get; set; }
        public long TenderFilledFormKeyId { get; set; }
        [ForeignKey("TenderFilledFormKeyId"), InverseProperty("TenderFilledFormsValues")]
        public TenderFilledFormKey TenderFilledFormKey { get; set; }
        [Required, MaxLength(4000)]
        public string Value { get; set; }
        public int? TenderProposalFormJsonConfigId { get; set; }
        [ForeignKey("TenderProposalFormJsonConfigId"), InverseProperty("TenderFilledFormsValues")]
        public TenderProposalFormJsonConfig TenderProposalFormJsonConfig { get; set; }
    }
}
