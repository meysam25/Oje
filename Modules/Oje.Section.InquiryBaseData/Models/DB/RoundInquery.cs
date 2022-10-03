using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("RoundInqueries")]
    public class RoundInquery: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Format { get; set; }
        public RoundInqueryType Type { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId"), InverseProperty("RoundInqueries")]
        public ProposalForm ProposalForm { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("RoundInqueries")]
        public SiteSetting SiteSetting { get; set; }
    }
}
