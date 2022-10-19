using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.SalesNetworkBaseData.Models.DB
{
    [Table("ProposalForms")]
    public class ProposalForm
    {
        public ProposalForm()
        {
            SalesNetworkProposalForms = new ();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("ProposalForm")]
        public List<SalesNetworkProposalForm> SalesNetworkProposalForms { get; set; }

    }
}
