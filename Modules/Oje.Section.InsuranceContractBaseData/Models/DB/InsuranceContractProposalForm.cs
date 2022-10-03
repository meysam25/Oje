using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractProposalForms")]
    public class InsuranceContractProposalForm: IEntityWithSiteSettingId
    {
        public InsuranceContractProposalForm()
        {
            InsuranceContracts = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string JsonConfig { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
        [Required, MinLength(4000)]
        public string TermTemplate { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("InsuranceContractProposalForms")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("InsuranceContractProposalForm")]
        public List<InsuranceContract> InsuranceContracts { get; set; }
    }
}
