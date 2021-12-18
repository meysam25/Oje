using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Models.DB
{
    [Table("PaymentMethodCompanies")]
    public class PaymentMethodCompany
    {
        [Key]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("PaymentMethodCompanies")]
        public Company Company { get; set; }
        [Key]
        public int PaymentMethodId { get; set; }
        [ForeignKey("PaymentMethodId"), InverseProperty("PaymentMethodCompanies")]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
