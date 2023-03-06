using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("ProposalFilledFormStatusLogs")]
    public class ProposalFilledFormStatusLog: SignatureEntity
    {
        public ProposalFilledFormStatusLog()
        {
            ProposalFilledFormStatusLogFiles = new();
        }

        [Key]
        public long Id { get; set; }
        public long ProposalFilledFormId { get; set; }
        [ForeignKey("ProposalFilledFormId")]
        [InverseProperty("ProposalFilledFormStatusLogs")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        public ProposalFilledFormStatus Type { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public long UserId { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; }

        [InverseProperty("ProposalFilledFormStatusLog")]
        public List<ProposalFilledFormStatusLogFile> ProposalFilledFormStatusLogFiles { get; set; }
    }
}
