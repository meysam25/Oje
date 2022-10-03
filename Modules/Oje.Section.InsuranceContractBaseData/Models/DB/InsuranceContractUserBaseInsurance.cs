using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractUserBaseInsurances")]
    public class InsuranceContractUserBaseInsurance: IEntityWithSiteSettingId
    {
        public InsuranceContractUserBaseInsurance()
        {
            InsuranceContractUsers = new List<InsuranceContractUser>();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(20)]
        public string Code { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("InsuranceContractUserBaseInsurances")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("InsuranceContractUserBaseInsurance")]
        public List<InsuranceContractUser> InsuranceContractUsers { get; set; }
    }
}
