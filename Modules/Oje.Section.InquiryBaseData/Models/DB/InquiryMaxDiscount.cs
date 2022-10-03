using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("InquiryMaxDiscounts")]
    public class InquiryMaxDiscount: IEntityWithSiteSettingId
    {
        public InquiryMaxDiscount()
        {
            InquiryMaxDiscountCompanies = new List<InquiryMaxDiscountCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public int Percent { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("InquiryMaxDiscounts")]
        public ProposalForm ProposalForm { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("InquiryMaxDiscounts")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("InquiryMaxDiscount")]
        public List<InquiryMaxDiscountCompany> InquiryMaxDiscountCompanies { get; set; }
    }
}
