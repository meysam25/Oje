using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("InquiryDurationCompanies")]
    public class InquiryDurationCompany
    {
        [Key]
        public long Id { get; set; }
        public int InquiryDurationId { get; set; }
        [ForeignKey("InquiryDurationId")]
        [InverseProperty("InquiryDurationCompanies")]
        public InquiryDuration InquiryDuration { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("InquiryDurationCompanies")]
        public Company Company { get; set; }
    }
}
