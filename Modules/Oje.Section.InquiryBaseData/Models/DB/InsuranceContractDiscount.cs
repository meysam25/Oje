using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("InsuranceContractDiscounts")]
    public class InsuranceContractDiscount: IEntityWithSiteSettingId
    {
        public InsuranceContractDiscount()
        {
            InsuranceContractDiscountCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        public int InsuranceContractId { get; set; }
        [ForeignKey("InsuranceContractId")]
        [InverseProperty("InsuranceContractDiscounts")]
        public InsuranceContract InsuranceContract { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("InsuranceContractDiscounts")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("InsuranceContractDiscount")]
        public List<InsuranceContractDiscountCompany> InsuranceContractDiscountCompanies { get; set; }

    }
}
