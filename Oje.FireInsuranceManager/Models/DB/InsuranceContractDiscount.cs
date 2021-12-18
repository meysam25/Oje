using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Models.DB
{
    [Table("InsuranceContractDiscounts")]
    public class InsuranceContractDiscount
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

        [InverseProperty("InsuranceContractDiscount")]
        public List<InsuranceContractDiscountCompany> InsuranceContractDiscountCompanies { get; set; }

    }
}
