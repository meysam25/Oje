using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContracts")]
    public class InsuranceContract : EntityWithCreateUser<User, long>
    {
        public InsuranceContract()
        {
            InsuranceContractValidUserForFullDebits = new ();
            InsuranceContractUsers = new();
            InsuranceContractProposalFilledForms = new();
            InsuranceContractInsuranceContractTypeMaxPrices = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public long Code { get; set; }
        public int InsuranceContractCompanyId { get; set; }
        [ForeignKey("InsuranceContractCompanyId")]
        [InverseProperty("InsuranceContracts")]
        public InsuranceContractCompany InsuranceContractCompany { get; set; }
        public int? InsuranceContractProposalFormId { get; set; }
        [ForeignKey("InsuranceContractProposalFormId"), InverseProperty("InsuranceContracts")]
        public InsuranceContractProposalForm InsuranceContractProposalForm { get; set; }
        public int? ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId"), InverseProperty("InsuranceContracts")]
        public ProposalForm ProposalForm { get; set; }
        public long MonthlyPrice { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        [MaxLength(200)]
        public string ContractDocumentUrl { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("CreateUserInsuranceContracts")]
        public User CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        [InverseProperty("UpdateUserInsuranceContracts")]
        public User UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("InsuranceContract")]
        public List<InsuranceContractValidUserForFullDebit> InsuranceContractValidUserForFullDebits { get; set; }
        [InverseProperty("InsuranceContract")]
        public List<InsuranceContractUser> InsuranceContractUsers { get; set; }
        [InverseProperty("InsuranceContract")]
        public List<InsuranceContractInsuranceContractType> InsuranceContractInsuranceContractTypes { get; set; }
        [InverseProperty("InsuranceContract")]
        public List<InsuranceContractTypeRequiredDocument> InsuranceContractTypeRequiredDocuments { get; set; }
        [InverseProperty("InsuranceContract")]
        public List<InsuranceContractProposalFilledForm> InsuranceContractProposalFilledForms { get; set; }
        [InverseProperty("InsuranceContract")]
        public List<InsuranceContractInsuranceContractTypeMaxPrice> InsuranceContractInsuranceContractTypeMaxPrices { get; set; }
    }
}
