using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            InquiryDurationCompanies = new ();
            CashPayDiscountCompanies = new ();
            InquiryMaxDiscountCompanies = new ();
            GlobalDiscountCompanies = new ();
            InsuranceContractDiscountCompanies = new();
            InquiryCompanyLimitCompanies = new();
            InqueryDescriptionCompanies = new();
            NoDamageDiscountCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("Company")]
        public List<InquiryDurationCompany> InquiryDurationCompanies { get; set; }
        [InverseProperty("Company")]
        public List<CashPayDiscountCompany> CashPayDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InquiryMaxDiscountCompany> InquiryMaxDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<GlobalDiscountCompany> GlobalDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InsuranceContractDiscountCompany> InsuranceContractDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InquiryCompanyLimitCompany> InquiryCompanyLimitCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InqueryDescriptionCompany> InqueryDescriptionCompanies { get; set; }
        [InverseProperty("Company")]
        public List<NoDamageDiscountCompany> NoDamageDiscountCompanies { get; set; }
    }
}
