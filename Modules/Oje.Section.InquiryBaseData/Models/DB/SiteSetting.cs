using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            InqueryDescriptions = new();
            CashPayDiscounts = new();
            GlobalDiscounts = new();
            InquiryCompanyLimits = new();
            InquiryDurations = new();
            InquiryMaxDiscounts = new();
            InsuranceContracts = new();
            InsuranceContractDiscounts = new();
            RoundInqueries = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<InqueryDescription> InqueryDescriptions { get; set; }
        [InverseProperty("SiteSetting")]
        public List<CashPayDiscount> CashPayDiscounts { get; set; }
        [InverseProperty("SiteSetting")]
        public List<GlobalDiscount> GlobalDiscounts { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InquiryCompanyLimit> InquiryCompanyLimits { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InquiryDuration> InquiryDurations { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InquiryMaxDiscount> InquiryMaxDiscounts { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContract> InsuranceContracts { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContractDiscount> InsuranceContractDiscounts { get; set; }
        [InverseProperty("SiteSetting")]
        public List<RoundInquery> RoundInqueries { get; set; }
    }
}
