using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Oje.Section.CarThirdBaseData.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount> ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts { get; set; }
    }
}
