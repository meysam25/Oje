using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("InquiryDurations")]
    public class InquiryDuration: IEntityWithSiteSettingId
    {
        public InquiryDuration()
        {
            InquiryDurationCompanies = new List<InquiryDurationCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public int Percent { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("InquiryDurations")]
        public ProposalForm ProposalForm { get; set; }
        public int Day { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("InquiryDurations")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("InquiryDuration")]
        public List<InquiryDurationCompany> InquiryDurationCompanies { get; set; }
    }
}
