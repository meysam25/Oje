using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("GlobalDiscountCompanies")]
    public class GlobalDiscountCompany
    {
        [Key]
        public long Id { get; set; }
        public int GlobalDiscountId { get; set; }
        [ForeignKey("GlobalDiscountId")]
        [InverseProperty("GlobalDiscountCompanies")]
        public GlobalDiscount GlobalDiscount { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("GlobalDiscountCompanies")]
        public Company Company { get; set; }
    }
}
