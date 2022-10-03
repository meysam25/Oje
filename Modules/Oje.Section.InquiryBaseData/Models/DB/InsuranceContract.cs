using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("InsuranceContracts")]
    public class InsuranceContract : EntityWithCreateUser<User, long>, IEntityWithSiteSettingId
    {
        public InsuranceContract()
        {
            InsuranceContractDiscounts = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("InsuranceContracts")]
        public ProposalForm ProposalForm { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId"), InverseProperty("InsuranceContracts")]
        public User CreateUser { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("InsuranceContracts")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("InsuranceContract")]
        public List<InsuranceContractDiscount> InsuranceContractDiscounts { get; set; }

    }
}
