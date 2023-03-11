using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
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
        public DateTime CreateDate { get; set; }
        public DateTime IssueDate { get; set; }
        public long UserId { get; set; }
        [Required, MaxLength(50)]
        public string Number { get; set; }
        [Required, MaxLength(200)]
        public string FileUrl { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
    }
}
