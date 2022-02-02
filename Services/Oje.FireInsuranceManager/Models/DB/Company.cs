using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            FireInsuranceCoverageCompanies = new ();
            FireInsuranceRateCompanies = new ();
            InquiryDurationCompanies = new ();
            InsuranceContractDiscountCompanies = new();
            InquiryMaxDiscountCompanies = new();
            InquiryCompanyLimitCompanies = new();
            GlobalDiscountCompanies = new();
            GlobalInqueries = new();
            CashPayDiscountCompanies = new();
            PaymentMethodCompanies = new();
            InqueryDescriptionCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Pic { get; set; }
        [MaxLength(100)]
        public string Pic32 { get; set; }
        [MaxLength(100)]
        public string Pic64 { get; set; }

        [InverseProperty("Company")]
        public List<FireInsuranceCoverageCompany> FireInsuranceCoverageCompanies { get; set; }
        [InverseProperty("Company")]
        public List<FireInsuranceRateCompany> FireInsuranceRateCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InquiryDurationCompany> InquiryDurationCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InsuranceContractDiscountCompany> InsuranceContractDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InquiryMaxDiscountCompany> InquiryMaxDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InquiryCompanyLimitCompany> InquiryCompanyLimitCompanies { get; set; }
        [InverseProperty("Company")]
        public List<GlobalDiscountCompany> GlobalDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<GlobalInquery> GlobalInqueries { get; set; }
        [InverseProperty("Company")]
        public List<CashPayDiscountCompany> CashPayDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<PaymentMethodCompany> PaymentMethodCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InqueryDescriptionCompany> InqueryDescriptionCompanies { get; set; }
    }
}
