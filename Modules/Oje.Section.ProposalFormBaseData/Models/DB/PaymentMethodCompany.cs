using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.DB
{
    [Table("PaymentMethodCompanies")]
    public class PaymentMethodCompany
    {
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("PaymentMethodCompanies")]
        public Company Company { get; set; }
        public int PaymentMethodId { get; set; }
        [ForeignKey("PaymentMethodId"), InverseProperty("PaymentMethodCompanies")]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
