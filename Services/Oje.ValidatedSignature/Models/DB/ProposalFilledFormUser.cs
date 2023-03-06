using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("ProposalFilledFormUsers")]
    public class ProposalFilledFormUser: SignatureEntity
    {
        [Key, Column(Order = 1)]
        public long ProposalFilledFormId { get; set; }
        [ForeignKey("ProposalFilledFormId"), InverseProperty("ProposalFilledFormUsers")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        [Key, Column(Order = 2)]
        public long UserId { get; set; }
        [Key, Column(Order = 3)]
        public ProposalFilledFormUserType Type { get; set; }
        public long? FromUserId { get; set; }
    }
}
