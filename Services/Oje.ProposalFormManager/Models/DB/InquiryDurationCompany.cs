using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
