﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("InquiryCompanyLimitCompanies")]
    public class InquiryCompanyLimitCompany
    {
        [Key, Column(Order = 1)]
        public int InquiryCompanyLimitId { get; set; }
        [ForeignKey("InquiryCompanyLimitId"), InverseProperty("InquiryCompanyLimitCompanies")]
        public InquiryCompanyLimit InquiryCompanyLimit { get; set; }
        [Key, Column(Order = 2)]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("InquiryCompanyLimitCompanies")]
        public Company Company { get; set; }
    }
}
