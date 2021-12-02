using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.SalesNetworkBaseData.Models.DB
{
    [Table("SalesNetworkProposalForms")]
    public class SalesNetworkProposalForm
    {
        [Key]
        public long Id { get; set; }
        public int SalesNetworkId { get; set; }
        [ForeignKey("SalesNetworkId")]
        [InverseProperty("SalesNetworkProposalForms")]
        public SalesNetwork SalesNetwork { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("SalesNetworkProposalForms")]
        public ProposalForm ProposalForm { get; set; }
    }
}
