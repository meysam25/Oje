using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractUserSubCategories")]
    public class InsuranceContractUserSubCategory: IEntityWithSiteSettingId
    {
        public InsuranceContractUserSubCategory()
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
        [ForeignKey("SiteSettingId"), InverseProperty("InsuranceContractUserSubCategories")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("InsuranceContractUserSubCategory")]
        public List<InsuranceContractUser> InsuranceContractUsers { get; set; }
    }
}
