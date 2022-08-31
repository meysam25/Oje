using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
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
