using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("ProposalFilledFormUsers")]
    public class ProposalFilledFormUser
    {
        [Key, Column(Order = 1)]
        public long ProposalFilledFormId { get; set; }
        [ForeignKey("ProposalFilledFormId"), InverseProperty("ProposalFilledFormUsers")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        [Key, Column(Order = 2)]
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("ProposalFilledFormUsers")]
        public User User { get; set; }
        [Key, Column(Order = 3)]
        public ProposalFilledFormUserType Type { get; set; }
        public long? FromUserId { get; set; }
        [ForeignKey("FromUserId"), InverseProperty("FromUserProposalFilledFormUsers")]
        public User FromUser { get; set; }
    }
}
