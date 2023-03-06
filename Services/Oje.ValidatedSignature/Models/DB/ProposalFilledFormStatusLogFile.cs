using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("ProposalFilledFormStatusLogFiles")]
    public class ProposalFilledFormStatusLogFile: SignatureEntity
    {
        [Key]
        public long Id { get; set; }
        public long ProposalFilledFormStatusLogId { get; set; }
        [ForeignKey("ProposalFilledFormStatusLogId"), InverseProperty("ProposalFilledFormStatusLogFiles")]
        public ProposalFilledFormStatusLog ProposalFilledFormStatusLog { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(200)]
        public string FileUrl { get; set; }
        public int SiteSettingId { get; set; }
    }
}
