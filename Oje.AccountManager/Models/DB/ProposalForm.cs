using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Models.DB
{
    [Table("ProposalForms")]
    public class ProposalForm
    {
        public ProposalForm()
        {
            RoleProposalForms = new();
        }

        [Key]
        public int Id { get;set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("ProposalForm")]
        public List<RoleProposalForm> RoleProposalForms { get; set; }
    }
}
