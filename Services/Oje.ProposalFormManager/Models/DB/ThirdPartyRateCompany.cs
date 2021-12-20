using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
