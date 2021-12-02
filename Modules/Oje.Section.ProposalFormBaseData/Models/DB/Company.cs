using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            PaymentMethodCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        [InverseProperty("Company")]
        public List<PaymentMethodCompany> PaymentMethodCompanies { get; set; }

    }
}
