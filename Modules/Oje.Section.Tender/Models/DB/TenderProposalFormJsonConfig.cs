using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models.PageForms;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("TenderProposalFormJsonConfigs")]
    public class TenderProposalFormJsonConfig: IEntityWithSiteSettingId
    {
        public TenderProposalFormJsonConfig()
        {
            TenderFilledFormJsons = new();
            TenderFilledFormPFs = new();
            TenderFilledFormsValues = new();
            TenderFilledFormPrices = new();
            TenderFilledFormIssues = new();
        }

        [Key]
        public int Id { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId"), InverseProperty("TenderProposalFormJsonConfigs")]
        public ProposalForm ProposalForm { get; set; }
        [Required]
        public string JsonConfig { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("TenderProposalFormJsonConfigs")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("TenderProposalFormJsonConfig")]
        public List<TenderFilledFormJson> TenderFilledFormJsons { get; set; }
        [InverseProperty("TenderProposalFormJsonConfig")]
        public List<TenderFilledFormPF> TenderFilledFormPFs { get; set; }
        [InverseProperty("TenderProposalFormJsonConfig")]
        public List<TenderFilledFormsValue> TenderFilledFormsValues { get; set; }
        [InverseProperty("TenderProposalFormJsonConfig")]
        public List<TenderFilledFormPrice> TenderFilledFormPrices { get; set; }
        [InverseProperty("TenderProposalFormJsonConfig")]
        public List<TenderFilledFormIssue> TenderFilledFormIssues { get; set; }



        [NotMapped]
        public PageForm PageForm { get; set; }

    }
}
