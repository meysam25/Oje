using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContracts")]
    public class InsuranceContract : EntityWithCreateUser<User, long>
    {
        public InsuranceContract()
        {
            InsuranceContractValidUserForFullDebits = new ();
            InsuranceContractUsers = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public long Code { get; set; }
        public int InsuranceContractTypeId { get; set; }
        [ForeignKey("InsuranceContractTypeId")]
        [InverseProperty("InsuranceContracts")]
        public InsuranceContractType InsuranceContractType { get; set; }
        public int InsuranceContractCompanyId { get; set; }
        [ForeignKey("InsuranceContractCompanyId")]
        [InverseProperty("InsuranceContracts")]
        public InsuranceContractCompany InsuranceContractCompany { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("InsuranceContracts")]
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
    }
}
