using Oje.Infrastructure.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("TenderFilledFormIssues")]
    public class TenderFilledFormIssue: SignatureEntity
    {
        [Key]
        public long Id { get; set; }
        public long TenderFilledFormId { get; set; }
        [ForeignKey("TenderFilledFormId"), InverseProperty("TenderFilledFormIssues")]
        public TenderFilledForm TenderFilledForm { get; set; }
        public int TenderProposalFormJsonConfigId { get; set; }
        [ForeignKey("TenderProposalFormJsonConfigId"), InverseProperty("TenderFilledFormIssues")]
        public TenderProposalFormJsonConfig TenderProposalFormJsonConfig { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime IssueDate { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("TenderFilledFormIssues")]
        public User User { get; set; }
        [Required, MaxLength(50)]
        public string Number { get; set; }
        [Required, MaxLength(200)]
        public string FileUrl { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
    }
}
