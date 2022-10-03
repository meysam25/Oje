using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            InsuranceContracts = new();
            InsuranceContractCompanies = new();
            InsuranceContractInsuranceContractTypeMaxPrices = new();
            InsuranceContractProposalFilledForms = new();
            InsuranceContractProposalForms = new();
            InsuranceContractTypes = new();
            InsuranceContractTypeRequiredDocuments = new();
            InsuranceContractUsers = new();
            InsuranceContractUserBaseInsurances = new();
            InsuranceContractUserSubCategories = new();
            InsuranceContractValidUserForFullDebits = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<InsuranceContract> InsuranceContracts { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContractCompany> InsuranceContractCompanies { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContractInsuranceContractTypeMaxPrice> InsuranceContractInsuranceContractTypeMaxPrices { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContractProposalFilledForm> InsuranceContractProposalFilledForms { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContractProposalForm> InsuranceContractProposalForms { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContractType> InsuranceContractTypes { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContractTypeRequiredDocument> InsuranceContractTypeRequiredDocuments { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContractUser> InsuranceContractUsers { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContractUserBaseInsurance> InsuranceContractUserBaseInsurances { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContractUserSubCategory> InsuranceContractUserSubCategories { get; set; }
        [InverseProperty("SiteSetting")]
        public List<InsuranceContractValidUserForFullDebit> InsuranceContractValidUserForFullDebits { get; set; }
    }
}
