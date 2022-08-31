using Oje.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("InquiryCompanyLimits")]
    public class InquiryCompanyLimit
    {
        public InquiryCompanyLimit()
        {
            InquiryCompanyLimitCompanies = new();
        }
        
        [Key]
        public int Id { get; set; }
        public InquiryCompanyLimitType Type { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("InquiryCompanyLimit")]
        public List<InquiryCompanyLimitCompany> InquiryCompanyLimitCompanies { get; set; }

    }
}
