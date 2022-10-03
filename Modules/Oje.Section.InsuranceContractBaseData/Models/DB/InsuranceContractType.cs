using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractTypes")]
    public class InsuranceContractType : EntityWithCreateUser<User, long>, IEntityWithSiteSettingId
    {
        public InsuranceContractType()
        {
            InsuranceContractInsuranceContractTypes = new();
            InsuranceContractTypeRequiredDocuments = new();
            InsuranceContractProposalFilledFormUsers = new();
            InsuranceContractInsuranceContractTypeMaxPrices = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("CreateUserInsuranceContractTypes")]
        public User CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        [InverseProperty("UpdateUserInsuranceContractTypes")]
        public User UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("InsuranceContractTypes")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("InsuranceContractType")]
        public List<InsuranceContractInsuranceContractType> InsuranceContractInsuranceContractTypes { get; set; }
        [InverseProperty("InsuranceContractType")]
        public List<InsuranceContractTypeRequiredDocument> InsuranceContractTypeRequiredDocuments { get; set; }
        [InverseProperty("InsuranceContractType")]
        public List<InsuranceContractProposalFilledFormUser> InsuranceContractProposalFilledFormUsers { get; set; }
        [InverseProperty("InsuranceContractType")]
        public List<InsuranceContractInsuranceContractTypeMaxPrice> InsuranceContractInsuranceContractTypeMaxPrices { get; set; }
    }
}
