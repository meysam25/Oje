using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ProposalFilledFormSiteSettings")]
    public class ProposalFilledFormSiteSetting: SignatureEntity
    {
        public long ProposalFilledFormId { get; set; }
        [ForeignKey("ProposalFilledFormId"), InverseProperty("ProposalFilledFormSiteSettings")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId") ,InverseProperty("ProposalFilledFormSiteSettings")]
        public SiteSetting SiteSetting { get; set; }

    }
}
