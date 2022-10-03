using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.ProposalFormBaseData.Models.DB
{
    [Table("ProposalFormPostPrices")]
    public class ProposalFormPostPrice: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("ProposalFormPostPrices")]
        public ProposalForm ProposalForm { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("ProposalFormPostPrices")]
        public SiteSetting SiteSetting { get; set; }
    }
}
