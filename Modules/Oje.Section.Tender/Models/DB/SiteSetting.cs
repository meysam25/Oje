using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Oje.Section.Tender.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            TenderFilledForms = new();
            TenderProposalFormJsonConfigs = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<TenderFilledForm> TenderFilledForms { get; set; }
        [InverseProperty("SiteSetting")]
        public List<TenderProposalFormJsonConfig> TenderProposalFormJsonConfigs { get; set; }
    }
}
