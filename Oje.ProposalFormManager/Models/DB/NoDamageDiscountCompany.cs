using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("NoDamageDiscountCompanies")]
    public class NoDamageDiscountCompany
    {
        [Key]
        public long Id { get; set; }
        public int NoDamageDiscountId { get; set; }
        [ForeignKey("NoDamageDiscountId")]
        [InverseProperty("NoDamageDiscountCompanies")]
        public NoDamageDiscount NoDamageDiscount { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("NoDamageDiscountCompanies")]
        public Company Company { get; set; }
    }
}
