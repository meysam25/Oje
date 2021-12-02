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
        public string Code { get; set; }
        public DateTime? CashDate { get; set; }
        public int SiteSettingId { get; set; }
    }
}
