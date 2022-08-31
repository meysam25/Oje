using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ThirdPartyPassengerRateCompanies")]
    public class ThirdPartyPassengerRateCompany
    {
        [Key]
        public long Id { get; set; }
        public int ThirdPartyPassengerRateId { get; set; }
        [ForeignKey("ThirdPartyPassengerRateId")]
        [InverseProperty("ThirdPartyPassengerRateCompanies")]
        public ThirdPartyPassengerRate ThirdPartyPassengerRate { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("ThirdPartyPassengerRateCompanies")]
        public Company Company { get; set; }

    }
}
