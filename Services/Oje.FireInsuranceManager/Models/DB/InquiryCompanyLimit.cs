using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Models.DB
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
