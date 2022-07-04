using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.ProposalFormBaseData.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            PaymentMethodCompanies = new();
            UserCompanies = new();
            AgentReffers = new();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        [InverseProperty("Company")]
        public List<PaymentMethodCompany> PaymentMethodCompanies { get; set; }
        [InverseProperty("Company")]
        public List<UserCompany> UserCompanies { get; set; }
        [InverseProperty("Company")]
        public List<AgentReffer> AgentReffers { get; set; }

    }
}
