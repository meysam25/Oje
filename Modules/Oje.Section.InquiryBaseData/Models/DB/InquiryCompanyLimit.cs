using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("InquiryCompanyLimits")]
    public class InquiryCompanyLimit: IEntityWithSiteSettingId
    {
        public InquiryCompanyLimit()
        {
            InquiryCompanyLimitCompanies = new();
        }
        
        [Key]
        public int Id { get; set; }
        public InquiryCompanyLimitType Type { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId"), InverseProperty("CreateUserInquiryCompanyLimits")]
        public User CreateUser { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId"), InverseProperty("UpdateUserInquiryCompanyLimits")]
        public User UpdateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("InquiryCompanyLimits")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("InquiryCompanyLimit")]
        public List<InquiryCompanyLimitCompany> InquiryCompanyLimitCompanies { get; set; }

    }
}
