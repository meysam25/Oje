using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("ProposalFilledFormDocuments")]
    public class ProposalFilledFormDocument
    {
        [Key]
        public long Id { get; set; }
        public long ProposalFilledFormId { get;set; }
        [ForeignKey("ProposalFilledFormId"), InverseProperty("ProposalFilledFormDocuments")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        public ProposalFilledFormDocumentType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public long Price { get; set; }
        public DateTime? TargetDate { get; set; }
        public int? BankId { get; set; }
        [ForeignKey("BankId"), InverseProperty("ProposalFilledFormDocuments")]
        public Bank Bank { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        public DateTime? CashDate { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        [MaxLength(200)]
        public string MainFileSrc { get; set; }
        public int SiteSettingId { get; set; }
    }
}
