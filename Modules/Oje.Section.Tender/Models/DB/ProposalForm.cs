using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("ProposalForms")]
    public class ProposalForm
    {
        public ProposalForm()
        {
            TenderProposalFormJsonConfigs = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int ProposalFormCategoryId { get; set; }
        [ForeignKey("ProposalFormCategoryId"), InverseProperty("ProposalForms")]
        public ProposalFormCategory ProposalFormCategory { get; set; }
        public bool? IsActive { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("ProposalForm")]
        public List<TenderProposalFormJsonConfig> TenderProposalFormJsonConfigs { get; set; }
    }
}
