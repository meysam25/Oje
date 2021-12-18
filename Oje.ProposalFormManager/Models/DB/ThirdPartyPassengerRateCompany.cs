using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
