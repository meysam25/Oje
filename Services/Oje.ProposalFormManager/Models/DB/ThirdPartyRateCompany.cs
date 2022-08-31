using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ThirdPartyRateCompanies")]
    public class ThirdPartyRateCompany
    {
        [Key]
        public long Id { get; set; }
        public int ThirdPartyRateId { get; set; }
        [ForeignKey("ThirdPartyRateId")]
        [InverseProperty("ThirdPartyRateCompanies")]
        public ThirdPartyRate ThirdPartyRate { get; set; }
        public int CompanyId { get; set; }
    }
}
