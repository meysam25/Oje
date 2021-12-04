using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("CarSpecificationAmountCompanies")]
    public class CarSpecificationAmountCompany
    {
        [Key]
        public long Id { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("CarSpecificationAmountCompanies")]
        public Company Company { get; set; }
        public int CarSpecificationAmountId { get; set; }
        [ForeignKey("CarSpecificationAmountId")]
        [InverseProperty("CarSpecificationAmountCompanies")]
        public CarSpecificationAmount CarSpecificationAmount { get; set; }
    }
}
