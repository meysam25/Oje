using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("InsuranceContractDiscountCompanies")]
    public class InsuranceContractDiscountCompany
    {
        [Key]
        public long Id { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("InsuranceContractDiscountCompanies")]
        public Company Company { get; set; }
        public int InsuranceContractDiscountId { get; set; }
        [ForeignKey("InsuranceContractDiscountId")]
        [InverseProperty("InsuranceContractDiscountCompanies")]
        public InsuranceContractDiscount InsuranceContractDiscount { get; set; }

    }
}
