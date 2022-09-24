using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            ThirdPartyRateCompanies = new List<ThirdPartyRateCompany>();
            ThirdPartyRequiredFinancialCommitmentCompanies = new List<ThirdPartyRequiredFinancialCommitmentCompany>();
            ThirdPartyExteraFinancialCommitmentComs = new List<ThirdPartyExteraFinancialCommitmentCom>();
            ThirdPartyPassengerRateCompanies = new List<ThirdPartyPassengerRateCompany>();
            ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("Company")]
        public List<ThirdPartyRateCompany> ThirdPartyRateCompanies { get; set; }
        [InverseProperty("Company")]
        public List<ThirdPartyRequiredFinancialCommitmentCompany> ThirdPartyRequiredFinancialCommitmentCompanies { get; set; }
        [InverseProperty("Company")]
        public List<ThirdPartyExteraFinancialCommitmentCom> ThirdPartyExteraFinancialCommitmentComs { get; set; }
        [InverseProperty("Company")]
        public List<ThirdPartyPassengerRateCompany> ThirdPartyPassengerRateCompanies { get; set; }
        [InverseProperty("Company")]
        public List<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompany> ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies { get; set; }
    }
}
