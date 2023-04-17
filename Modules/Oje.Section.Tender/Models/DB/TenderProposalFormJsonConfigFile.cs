using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("TenderProposalFormJsonConfigFiles")]
    public class TenderProposalFormJsonConfigFile
    {
        [Key]
        public int Id { get; set; }
        public int TenderProposalFormJsonConfigId { get; set; }
        [ForeignKey("TenderProposalFormJsonConfigId"), InverseProperty("TenderProposalFormJsonConfigFiles")]
        public TenderProposalFormJsonConfig TenderProposalFormJsonConfig { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }

    }
}
