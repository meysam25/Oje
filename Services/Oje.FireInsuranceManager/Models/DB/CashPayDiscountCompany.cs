using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Models.DB
{
    [Table("CashPayDiscountCompanies")]
    public class CashPayDiscountCompany
    {
        [Key]
        public long Id { get; set; }
        public int CashPayDiscountId { get; set; }
        [ForeignKey("CashPayDiscountId")]
        [InverseProperty("CashPayDiscountCompanies")]
        public CashPayDiscount CashPayDiscount { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("CashPayDiscountCompanies")]
        public Company Company { get; set; }
    }
}
