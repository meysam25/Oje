using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Models.DB
{
    [Table("RoleProposalForms")]
    public class RoleProposalForm
    {
        [Key]
        public int RoleId { get; set; }
        [ForeignKey("RoleId"), InverseProperty("RoleProposalForms")]
        public Role Role { get; set; }
        [Key]
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId"), InverseProperty("RoleProposalForms")]
        public ProposalForm ProposalForm { get; set; }
    }
}
